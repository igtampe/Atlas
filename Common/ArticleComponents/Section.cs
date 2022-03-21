using Atlas.Common.ArticleComponents.SectionComponents;
using System.Text.RegularExpressions;

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
        /// <returns></returns>
        public static Section MakeSection(string Text, int Level = 0) {

            Text = Text.TrimStart();

            Section S = new();

            //The first line is the title
            int TLine = Text.IndexOf(Environment.NewLine);
            if (TLine == -1) {
                //I guess the only line is a title.
                S.Title = Text;
                return S;
            }

            S.Title = Text[0..TLine].TrimStart().TrimEnd();
            Text = "\n\n" + Text[(TLine + 1)..];

            S.Components = new();

            Match SubsectionMatch = Regex.Match(Text, "\\n=+");
            string SectionText = Text;
            string? SubsectionText = null;
            if (SubsectionMatch.Success) {
                SectionText = Text[0..(SubsectionMatch.Index - 1)];
                SubsectionText = Text[SubsectionMatch.Index..];
            }

            S.Components.AddRange(SectionTextProcessor(SectionText));
            if (SubsectionText is not null) { S.Components.AddRange(SectionSubsectionProcessor(SubsectionText, Level)); }
            
            return S;

        }

        private static List<object> SectionTextProcessor(string Text) {

            List<object> O = new();

            while (string.IsNullOrWhiteSpace(Text)) {

                Match CurMatch = Regex.Match(Text, "\\n\\n(\\|\\||\\|-\\||\\[|-|#)");
                if (CurMatch.Success) {

                    if (CurMatch.Index != 0) {

                        //Assume everything before is a paragraph
                        O.Add(Paragraph.MakeParagraph(Text[0..(CurMatch.Index - 1)]));
                        Text = Text[CurMatch.Index..];

                    }

                    Match NextMatch = Regex.Match(Text[(CurMatch.Index + 1)..], "\\n\\n(\\|\\||\\|-\\||\\[IMG|-|#)");
                    string CurText = NextMatch.Success ? Text[0..NextMatch.Index] : Text;

                    switch (CurMatch.Value) {
                        case "\n\n||":
                            //We have a table
                            O.Add(Table.MakeTable(CurText));
                            break;
                        case "\n\n|-|":
                            //We have an ImageGrid
                            O.Add(ImageGrid.MakeGrid(CurText));
                            break;
                        case "\n\n-":
                        case "\n\n#":
                            //We have some sort of bullet list
                            O.Add(BulletList.MakeList(CurText));
                            break;
                        default:
                            //Probably just a bracket (which may be an image) or... maybe something we implement in the future but won't be supported by this section
                            //In any case, it's going as a paragraph
                            O.Add(Paragraph.MakeParagraph(CurText));
                            break;
                    }
                } else {
                    //There are no more special characters. with what's left and return
                    O.Add(Paragraph.MakeParagraph(Text));
                    return O;
                }
            }
            
            return O;

        }

        private static List<object> SectionSubsectionProcessor(string Text, int Level) {
            
            List<object> O = new();

            while (!string.IsNullOrWhiteSpace(Text)) {

                //We can assume the text already starts with a section

                Match NextMatch = Regex.Match(Text, "\\n={" + Level + "}[^=]");
                string Subsection = NextMatch.Success ? Text[0..(NextMatch.Index - 1)] : Text;

                O.Add(MakeSection(Subsection));

                Text = Text[NextMatch.Index..];

            }
            
            return O;

        }
    }
}
