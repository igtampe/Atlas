using Igtampe.BasicLogger;
using System.Text.RegularExpressions;

namespace Atlas.Common.ArticleComponents {

    /// <summary>Section of an article</summary>
    public static class Parser {

        /// <summary>Parses a section</summary>
        /// <param name="Text"></param>
        /// <param name="Level"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static List<object> ParseText(string Text, int Level = 0, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug("Commencing parsing of text");
            
            Text = Text.TrimStart();

            List<object> Components = new();

            Components.AddRange(SectionTextProcessor(Text, GlobalLogger));

            return Components;

        }

        private static List<object> SectionTextProcessor(string Text, Logger? GlobalLogger = null) {

            List<object> O = new();

            while (!string.IsNullOrWhiteSpace(Text)) {

                Text = Text.TrimStart();

                GlobalLogger?.Debug($"{Text.Length} characters remaining");

                //Assume we start at a line with something on it.
                int EndElement = Text[0] == '=' ? Text.IndexOf("\r\n") : Text.IndexOf("\r\n\r\n");
                EndElement = EndElement < 0 ? Text.Length : EndElement;

                string Element = Text[0..EndElement];
                //We now have the bounds of the element

                Match CurMatch = Regex.Match(Element.Length >= 3 ? Element[0..2] : Text, "(\\|\\||\\|-\\||\\[|-|#|=)");

                if (CurMatch.Success) {

                    GlobalLogger?.Debug("Found a special character");

                    switch (CurMatch.Value) {
                        case "=":
                            //We have a section Header
                            GlobalLogger?.Debug($"Found a Section Header");
                            O.Add(SectionHeader.MakeHeader(Element, GlobalLogger));
                            break;
                        case "||":
                            //We have a table
                            GlobalLogger?.Debug($"Found a Table");
                            O.Add(Table.MakeTable(Element, GlobalLogger));
                            break;
                        case "|-|":
                            //We have an ImageGrid
                            GlobalLogger?.Debug($"Found an ImageGrid");
                            O.Add(ImageGrid.MakeGrid(Element, GlobalLogger));
                            break;
                        case "-":
                        case "#":
                            //We have some sort of bullet list
                            GlobalLogger?.Debug($"Found a List");
                            O.Add(BulletList.MakeList(Element, GlobalLogger));
                            break;
                        default:
                            //Probably just a bracket (which may be an image) or... maybe something we implement in the future but won't be supported by this section
                            //In any case, it's going as a paragraph
                            GlobalLogger?.Debug($"Found a Paragraph, or found another type of element we don't know how to parse");
                            O.Add(Paragraph.MakeParagraph(Element, GlobalLogger));
                            break;
                    }

                } else {
                    //There are no more special characters. with what's left and return
                    GlobalLogger?.Debug($"No special characters were found. Assuming a paragraph");
                    O.Add(Paragraph.MakeParagraph(Element,GlobalLogger));
                }

                Text = Text[EndElement..];

            }
            
            return O;

        }
    }
}
