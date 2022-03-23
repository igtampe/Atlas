using Atlas.Common.ArticleComponents.SectionComponents;
using System.Text.RegularExpressions;
using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents{

    /// <summary>Section of an article</summary>
    public class Section {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "SECTION";

        /// <summary>Title of this section</summary>
        public string Title { get; set; } = "";

        /// <summary>List of elements that will go on this section</summary>
        public List<object>? Components { get; set; } = null;
        //I hope the JSON Serializer likes this. Even if this is a bit breaking from separation of concerns, all of these objects have basically nothing
        //in common anyway, and we're not really going to be doing anything with them in the frontend from anything above section anyway so zoop.

        /// <summary>Parses a section</summary>
        /// <param name="Text"></param>
        /// <param name="Level"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static Section MakeSection(string Text, int Level = 0, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug("Commencing parsing of new section");
            
            Text = Text.TrimStart();

            Section S = new();

            //The first line is the title EXCEPT if Level is 0
            if (Level != 0) {
                int TLine = Text.IndexOf(Environment.NewLine);
                if (TLine == -1) {
                    //I guess the only line is a title.
                    S.Title = Text;

                    GlobalLogger?.Info($"Section {S.Title} has only one line. Assuming its just a title");
                    return S;
                }

                S.Title = Text[0..TLine].TrimStart().TrimEnd();
                Text = "\r\n\r\n" + Text[(TLine + 1)..];
            } else {
                S.Title = "MAIN_SECTION";
                Text = "\r\n\r\n" + Text;
            }

            GlobalLogger?.Debug($"Parsing {S.Title}");

            S.Components = new();

            Match SubsectionMatch = Regex.Match(Text, "\r\n=+");
            string SectionText = Text;
            string? SubsectionText = null;
            if (SubsectionMatch.Success) {
                GlobalLogger?.Debug($"{S.Title} has subsections starting at {SubsectionMatch.Index}");
                SectionText = Text[0..(SubsectionMatch.Index - 2)];
                SubsectionText = Text[SubsectionMatch.Index..];
            }

            GlobalLogger?.Debug($"{S.Title} Processing Text");
            S.Components.AddRange(SectionTextProcessor(SectionText, GlobalLogger));

            GlobalLogger?.Debug($"{S.Title} Processing Subsections");
            if (SubsectionText is not null) { S.Components.AddRange(SectionSubsectionProcessor(SubsectionText, Level, GlobalLogger)); }

            GlobalLogger?.Debug($"{S.Title} Wrapping Up");

            return S;

        }

        private static List<object> SectionTextProcessor(string Text, Logger? GlobalLogger = null) {

            List<object> O = new();

            while (!string.IsNullOrWhiteSpace(Text)) {

                GlobalLogger?.Debug($"Checking for any special characters. {Text.Length} characters remaining");

                Match CurMatch = Regex.Match(Text, "\r\n\r\n(\\|\\||\\|-\\||\\[|-|#)");
                if (CurMatch.Success) {

                    GlobalLogger?.Debug("Found a special character");

                    if (CurMatch.Index != 0) {

                        GlobalLogger?.Debug("Not at index 0. Adding a paragraph before");

                        //Assume everything before is a paragraph
                        O.Add(Paragraph.MakeParagraph(Text[0..(CurMatch.Index - 1)], GlobalLogger));
                        Text = Text[CurMatch.Index..];

                    }

                    Match NextMatch = Regex.Match(Text[(CurMatch.Index + 1)..], "\r\n\r\n(\\|\\||\\|-\\||\\[IMG|-|#)");
                    string CurText = NextMatch.Success ? Text[0..NextMatch.Index] : Text;
                    CurText = CurText.Trim();
                    Text = Text[CurText.Length..].Trim();

                    GlobalLogger?.Debug($"Determining what this element is");

                    switch (CurMatch.Value) {
                        case "\r\n\r\n||":
                            //We have a table
                            GlobalLogger?.Debug($"Found a Table");
                            O.Add(Table.MakeTable(CurText, GlobalLogger));
                            break;
                        case "\r\n\r\n|-|":
                            //We have an ImageGrid
                            GlobalLogger?.Debug($"Found an ImageGrid");
                            O.Add(ImageGrid.MakeGrid(CurText, GlobalLogger));
                            break;
                        case "\r\n\r\n-":
                        case "\r\n\r\n#":
                            //We have some sort of bullet list
                            GlobalLogger?.Debug($"Found a List");
                            O.Add(BulletList.MakeList(CurText,GlobalLogger));
                            break;
                        default:
                            //Probably just a bracket (which may be an image) or... maybe something we implement in the future but won't be supported by this section
                            //In any case, it's going as a paragraph
                            GlobalLogger?.Debug($"Found a Paragraph, or found another type of element we don't know how to parse");
                            O.Add(Paragraph.MakeParagraph(CurText, GlobalLogger));
                            break;
                    }
                } else {
                    //There are no more special characters. with what's left and return
                    GlobalLogger?.Debug($"No special characters were found. Assuming the rest is a paragraph, and returning");
                    O.Add(Paragraph.MakeParagraph(Text.TrimStart(),GlobalLogger));
                    return O;
                }
            }
            
            return O;

        }

        private static List<object> SectionSubsectionProcessor(string Text, int Level, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Processing Subsections");

            List<object> O = new();
            Text = Text.TrimStart();
            string Expression = "\r\n={" + Level + "}[^=]";
            Match LastMatch = Regex.Match(Text, Expression);

            while (!string.IsNullOrWhiteSpace(Text)) {

                //We can assume the text already starts with a section
                GlobalLogger?.Debug($"Processing Subsections. {Text.Length} Characters Remaining.");

                Match NextMatch = Regex.Match(Text[LastMatch.Index..], Expression);
                string Subsection = NextMatch.Success ? Text[0..(NextMatch.Index + LastMatch.Index)] : Text;
                Subsection = Subsection.Trim();
                Text = Text[NextMatch.Index..].Trim();
                LastMatch = NextMatch;

                GlobalLogger?.Debug($"Creating Subsection");
                O.Add(MakeSection(Subsection,Level, GlobalLogger));

            }
            
            return O;

        }
    }
}
