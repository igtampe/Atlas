using Igtampe.BasicLogger;
using System.Text.RegularExpressions;

namespace Atlas.Common.ArticleComponents {

    /// <summary>Formatted text element which can be used to show text as a link, and bold/italic/underlined.</summary>
    public class FormattedText {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "FORMATTED_TEXT";

        /// <summary>Whether or not this formatted text is bold</summary>
        public bool Bold { get; set; } = false;

        /// <summary>Whether or not this formatted text is italic</summary>
        public bool Italic { get; set; } = false;

        /// <summary>Whether or not this formatted text is underlined</summary>
        public bool Underline { get; set; } = false;

        /// <summary>Link this formatted text is supposed to have. If it's null, this is not a link</summary>
        public string? Link { get; set; } = null;

        /// <summary>Text to display</summary>
        public string Text { get; set; }

        /// <summary>Creates a single instance of formatted text</summary>
        /// <param name="Text"></param>
        private FormattedText(string Text) => this.Text = Text;

        /// <summary>Creates a list of formatted text objects. Used with blocks of text with varied formatting<br/>
        /// <br/>
        /// *This is italic*, **this is bold**, __ This is underline__. Mix and match as needed.<br/>
        /// [This is a link to google | https://www.google.com]. Links are not subprocessed (IE You cannot make a link bold by making its text bold. Put the modifiers around the link)
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Bold"></param>
        /// <param name="Italic"></param>
        /// <param name="Underline"></param>
        /// <returns></returns>
        public static FormattedText MakeLink(string Text, bool Bold, bool Italic, bool Underline) {

            //Split this into two by using the mighty power of the split command
            string[] LinkSplit = Text.Split('|');
            if (LinkSplit.Length != 2) {
                //there's been a problem. Return the text unformatted (with the [])
                return new($"[{Text}]") { Bold=Bold, Italic=Italic, Underline=Underline};
            }

            return new($"{LinkSplit[0].Trim()}") { Bold = Bold, Italic = Italic, Underline = Underline, Link = LinkSplit[1].Trim() };

        }

        /// <summary>Parses and separates a block of unformatted text into a list of formatted text items</summary>
        /// <param name="Text"></param>
        /// <param name="Bold"></param>
        /// <param name="Italic"></param>
        /// <param name="Underline"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static List<FormattedText> FormatText(string Text, bool Bold = false, bool Italic = false, bool Underline = false, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Formatting Text \"{Text}\"");

            List<FormattedText> L = new();

            while (!string.IsNullOrWhiteSpace(Text)) {
                
                GlobalLogger?.Debug($"Formatting Text. {Text.Length} characters remaining. Finding special characters");

                //Find the index of the next special bit of text:
                var SpecialCharsMatch = Regex.Match(Text, "_{2}|\\*+|\\["); //Checks for any string containing __, *, or [

                if (!SpecialCharsMatch.Success) {
                    GlobalLogger?.Debug($"Remaining text has no special characters. Adding remaining text and leaving");
                    //There are no more special characters. Add the remaining text to the list, and lets get out of here.
                    L.Add(new(Text) {  Bold = Bold, Italic = Italic, Underline = Underline, });
                    break;
                } else {

                    //There's a match
                    GlobalLogger?.Debug($"Found a special char");

                    //if it's index is not 0, we need to add everything before it as standard text:
                    if (SpecialCharsMatch.Index != 0) {
                        GlobalLogger?.Debug($"Adding text before the special char");
                        L.Add(new(Text[..SpecialCharsMatch.Index])); //Add the text before  the index of the special chars
                        Text=Text[SpecialCharsMatch.Index..]; //Remove that text from the text we're still processing
                    }

                    //Determine the next special chars we need to find:
                    string EndChars = SpecialCharsMatch.Value == "[" ? "]" : SpecialCharsMatch.Value;

                    //Search for the next match of the found 
                    //Since this is relatively simple we can just do IndexOf
                    int EndCharsIndex = Text[EndChars.Length..].IndexOf(EndChars);
                    if (EndCharsIndex == -1) {
                        //This is an unfinished bit of special text. We give up, and just add the rest, then return
                        GlobalLogger?.Error($"Unterminated special char. Returning remaining text and getting the heck out");
                        L.Add(new(Text));
                        Text = string.Empty;
                        break;
                    }

                    /*
                        We now have essentially selected the bit of special text we need to format.
                        It spans from SpecialCharsMatch.Index to (EndCharsIndex + EndChars.Length.)
                        The text to subprocess is from (SpecialCharsMatch.Index + SpecialCharsMatch.Value.Length) to EndCharsIndex.

                        The SpecialCharsMatch index is now wrong actually, because we removed anything before the match. Use 0 instead.
                        SpecialCharsMatch's value's length *should* match the one from EndChars, so we can use it interchangeably
                        The length of the substring we need is also coincidentally the endchars index - endchars' length
                        Or you know we can use range like VS suggests. Gracias a dios que that's a thing.

                        ***The text we want to subprocess [Because there may be a link | Link]*** More text we will process later
                           |-----------------------------------------------------------------| EndChars Lenght to EndChars Index
                           Range specified by EndCharsIndex + EndChars.Length                    |-------------------------------

                        According to this analysis:
                                 |*|**|***|__|__*|__**|__***|
                        ---------|-|--|---|--|---|----|-----|
                        BOLD     | |X | X |  |   | X  |  X  |
                        ITALIC   |X|  | X |  | X |    |  X  |
                        UNDERLINE| |  |   |X | X | X  |  X  |

                        Italic occurs if the specialchars are odd
                        Bold occurs when there's at least two *
                        Underline occurs when there's two Underscore

                     */

                    string SubText = Text[EndChars.Length..(EndCharsIndex+EndChars.Length)];

                    //Let's assess what operators we have to implement.
                    if (EndChars == "]") {
                        GlobalLogger?.Debug($"This is a Link. Adding the Link");
                        //THIS IS A LINK. DO NOT SUBPROCESS. ADD IT SINGLY.
                        L.Add(MakeLink(SubText, Bold, Italic, Underline));
                        

                    } else {

                        GlobalLogger?.Debug($"This is text formatting. Initiating subprocessing");

                        bool SubBold = EndChars.Contains("**") || Bold;
                        bool SubItalic = EndChars.Length % 2 == 1 || Italic;
                        bool SubUnderline = EndChars.Contains("__") || Underline;

                        //We subprocess the text
                        L.AddRange(FormatText(SubText,SubBold,SubItalic,SubUnderline,GlobalLogger));

                    }

                    //Then we remove the text we processed:
                    Text = Text[(EndCharsIndex + (EndChars.Length*2))..]; //And proceed
                    //Here we have to multiply by 2. The endCharsIndex is missing the matching char's length (which is the same as EndChar's length.
                    //The True EndChars index is essentially EndCharsIndex plus EndChars.length
                    //and we have to add EndChars Length again to move past the end chars.
                }
            }

            return L;
        
        }
    }
}
