using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents.SectionComponents {

    /// <summary>A grid of images</summary>
    public class ImageGrid {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "IMAGE_GRID";

        /// <summary>An ImageBox for the ImageGrid component</summary>
        public class ImageGridBox : ImageBox {

            /// <summary>Width in 12ths of the screen</summary>
            public int Width { get; set; } = 12;

            /// <summary>Makes an ImageGridBox</summary>
            /// <param name="Row"></param>
            /// <returns></returns>
            public static ImageGridBox MakeImageGridBox(string Row) {

                ImageGridBox I = new();

                //[IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2]
                //Get the substring between the beginning and the end.
                if (Row.Length < 2) { return I; }
                Row = Row[1..(Row.Length - 2)];

                //IMG | This is the alt text | https://avatars.githubusercontent.com/u/49919240 | HA HA HA | 2
                //Split the row
                string[] RowSplit = Row.Split('|');
                
                //Verify length
                if(RowSplit.Length != 5 ) { return I; }

                I.AltText = RowSplit[1].TrimStart().TrimEnd();
                I.ImageURL = RowSplit[2].TrimStart().TrimEnd();
                I.Description = RowSplit[3].TrimStart().TrimEnd();
                I.Width = int.TryParse(RowSplit[4].TrimStart().TrimEnd(), out int Width) ? Width : 1;

                return I;

            }
        }

        /// <summary>Title of this ImageGrid</summary>
        public string Title { get; set; } = "";

        /// <summary>Images in this ImageGrid</summary>
        public List<ImageGridBox> Images { get; set; } = new();

        /// <summary>Parses text into an ImageGrid</summary>
        /// <param name="Text"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static ImageGrid MakeGrid(string Text, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Creating an ImageGrid");

            ImageGrid G = new();

            //Get the title
            string[] RowTexts = Text.Split(Environment.NewLine);
            if (RowTexts.Length < 2) {
                GlobalLogger?.Error($"Insufficient rows to make an ImageGrid. Returning Error ImageGrid");
                G.Title = "Something went wrong!";
                return G;
            }

            //Title
            if (RowTexts[0].Length > 2) { G.Title = RowTexts[0][3..]; }
            
            GlobalLogger?.Debug($"Processing ImageGrid Rows");

            //header and other rows
            foreach (string RowText in RowTexts[2..]) { G.Images.Add(ImageGridBox.MakeImageGridBox(RowText)); }

            return G;
        
        }
    }
}
