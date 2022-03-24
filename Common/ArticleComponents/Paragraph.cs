using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents {
    
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
            /// <param name="GlobalLogger"></param>
            /// <returns></returns>
            public static ParagraphImageBox MakeParagraphImageBox(string Row, Logger? GlobalLogger = null) {
                ParagraphImageBox I = new();

                GlobalLogger?.Debug($"Creating Pargraph ImageBox");

                //[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
                //Get the substring between the beginning and the end.
                if (Row.Length < 2) { return I; }
                //Row = Row[1..(Row.Length - 2)]; //This is unnecessary. Its already done

                //IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2
                //Split the row
                string[] RowSplit = Row.Split('|');

                //Verify length
                if (RowSplit.Length < 4) {
                    GlobalLogger?.Error($"Image wasn't formatted properly. Returning blank image");
                    return I; 
                }

                I.AltText = RowSplit[1].TrimStart().TrimEnd();
                I.ImageURL = RowSplit[2].TrimStart().TrimEnd();
                I.Description = RowSplit[3].TrimStart().TrimEnd();
                I.Position = RowSplit.Length > 4 ? RowSplit[4].TrimStart().TrimEnd().ToUpper() switch {
                    "LEFT" => ImagePosition.LEFT,
                    "RIGHT" => ImagePosition.RIGHT,
                    "TOP" => ImagePosition.TOP,
                    _ => ImagePosition.NONE
                } : ImagePosition.NONE;

                GlobalLogger?.Debug($"Paragraph Image Processed");

                return I;
            }
        }

        /// <summary>ImageBox that this paragraph optionally has</summary>
        public ParagraphImageBox? Image { get; set; } = null;

        /// <summary>Formatted text of this Paragraph</summary>
        public List<FormattedText>? Text { get; set; } = null;

        /// <summary>Parses text to create a paragraph</summary>
        /// <param name="Text"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static Paragraph MakeParagraph(string Text, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Creating a Paragraph");

            Paragraph P = new();

            if (Text.Length > 5 && Text[0..4].ToUpper()=="[IMG") {
                
                GlobalLogger?.Debug($"Found a paragraph image");
                
                //Find the index of the closing ]
                int EB = Text.IndexOf(']');
                if (EB == -1) {

                    GlobalLogger?.Warn($"Paragraph image was for some reason not finished. Trimming the first [");
                    Text = Text[1..]; 
                }  else {
                    GlobalLogger?.Debug($"Making paragraph image box");
                    P.Image = ParagraphImageBox.MakeParagraphImageBox(Text[1..EB], GlobalLogger);
                    Text = Text[(EB + 1)..];
                }
            }

            GlobalLogger?.Debug($"Formatting Text...");
            P.Text = FormattedText.FormatText(Text,false,false,false,GlobalLogger);

            return P;
        }
    }
}
