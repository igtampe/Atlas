namespace Atlas.Common.ArticleComponents.SectionComponents {
    
    /// <summary>A Paragraph block of formattedText and an optional image</summary>
    public class Paragraph {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "PARAGRAPH";

        /// <summary>ImageBox for a paragraph image box</summary>
        public class ParagraphImageBox : ImageBox {

            /// <summary>Position of this image relative to the paragraph</summary>
            public enum ImagePosition { 
                /// <summary>No position. Display as a regular ImageBox</summary>
                NONE,
                /// <summary>Left of the paragraph</summary>
                LEFT,
                /// <summary>Right of the paragraph</summary>
                RIGHT,
                /// <summary>On top of the paragraph</summary>
                TOP
            }

            /// <summary>Position of this ImageBox</summary>
            public ImagePosition Position { get; set; }

            /// <summary>Parses the text of an ImageBox into a ParagraphImageBox</summary>
            /// <param name="Row"></param>
            /// <returns></returns>
            public static ParagraphImageBox MakeParagraphImageBox(string Row) {
                ParagraphImageBox I = new();

                //[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
                //Get the substring between the beginning and the end.
                if (Row.Length < 2) { return I; }
                Row = Row[1..(Row.Length - 2)];

                //IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2
                //Split the row
                string[] RowSplit = Row.Split('|');

                //Verify length
                if (RowSplit.Length < 4) { return I; }

                I.AltText = RowSplit[1].TrimStart().TrimEnd();
                I.ImageURL = RowSplit[2].TrimStart().TrimEnd();
                I.Description = RowSplit[3].TrimStart().TrimEnd();
                I.Position = RowSplit.Length > 4 ? RowSplit[4].TrimStart().TrimEnd().ToUpper() switch {
                    "LEFT" => ImagePosition.LEFT,
                    "RIGHT" => ImagePosition.RIGHT,
                    "TOP" => ImagePosition.TOP,
                    _ => ImagePosition.NONE
                } : ImagePosition.NONE;

                return I;
            }
        }

        /// <summary>ImageBox that this paragraph optionally has</summary>
        public ParagraphImageBox? Image { get; set; } = null;

        /// <summary>Formatted text of this Paragraph</summary>
        public List<FormattedText>? Text { get; set; } = null;

        /// <summary>Parses text to create a paragraph</summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static Paragraph MakeParagraph(string Text) {

            Paragraph P = new();

            if (Text.Length > 5 && Text[0..4].ToUpper()=="[IMG") {
                //Find the index of the closing ]
                int EB = Text.IndexOf(']');
                if (EB == -1) { Text = Text[1..]; } 
                else {  
                    P.Image = ParagraphImageBox.MakeParagraphImageBox(Text[1..EB]);
                    Text = Text[(EB + 1)..];
                }
            }

            P.Text = FormattedText.FormatText(Text);

            return P;
        }
    }
}
