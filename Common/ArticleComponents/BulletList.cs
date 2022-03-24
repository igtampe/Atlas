using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents {

    /// <summary>Ordered or unordered bullet-list of formatted text</summary>
    public class BulletList {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "LIST";

        /// <summary>BulletList Types</summary>
        public enum BulletListType { 
        
            /// <summary>Bullet. Default for bullet lists</summary>
            BULLET = '-',

            /// <summary>Numerical. Used for Ordered lists</summary>
            NUMERICAL = '#', 

        }

        /// <summary>An item in a bullet list with an optional sublist</summary>
        public class BulletListItem { 
        
            /// <summary>Text of this bullet list item</summary>
            public List<FormattedText> Text { get; set; } = new();

            /// <summary>Sublist this item has</summary>
            public BulletList? Sublist { get; set; } = null;

            /// <summary>Makes a bulletlist item</summary>
            /// <param name="Text"></param>
            /// <param name="Type"></param>
            /// <param name="GlobalLogger"></param>
            /// <returns></returns>
            public static BulletListItem MakeBulletListItem(string Text, BulletListType Type, Logger? GlobalLogger = null) {

                Text = Text.Trim();

                GlobalLogger?.Debug($"Processing Bullet Item");

                BulletListItem B = new();

                int Line1End = Text.IndexOf(Environment.NewLine);
                Line1End = Line1End == -1 ? Text.Length : Line1End;

                int IndexOfFirstBullet = Text.IndexOf((char)Type);
                if (IndexOfFirstBullet == -1) {

                    GlobalLogger?.Error($"Couldn't find the index of the first bullet. What the");

                    //uh...
                    B.Text = FormattedText.FormatText(Text);
                    return B;
                }

                //The text of the first bullet is from the index of the first bullet plus one, and the end of the line
                //Assume the first line is this bullet list item
                B.Text = FormattedText.FormatText(Text[(IndexOfFirstBullet + 1)..Line1End].TrimStart().TrimEnd());
                Text = Text[Line1End..];
                if (string.IsNullOrWhiteSpace(Text)) {
                    GlobalLogger?.Debug($"This was only a single bullet. Returning");
                    return B; 
                }

                GlobalLogger?.Debug($"There's more lines. Making a sublist");
                //OK we have more than one line, meaning we've got a sublist. 
                B.Sublist = MakeList(Text,GlobalLogger);

                return B;

            }
        }

        /// <summary>List of items in this bullet list</summary>
        public List<BulletListItem> Items { get; set; } = new();

        /// <summary>Type of this list</summary>
        public BulletListType Type { get; set; } = BulletListType.BULLET;

        /// <summary>Parses text into a bullet list</summary>
        /// <param name="Text"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static BulletList MakeList(string Text, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Making a List");

            BulletList L = new();

            //Check what type of list this is:
            BulletListType Type = (BulletListType)Text.TrimStart()[0];
            GlobalLogger?.Debug($"Found a list of type {Type}");
            L.Type = Type;

            int SearchIndent = Text.IndexOf((char)Type);
            if (SearchIndent == -1) {
                GlobalLogger?.Error($"What");
                return L; 
            } //There's a problem with the rest of it so *no*

            string SearchBullet = new(' ', SearchIndent);
            SearchBullet = Environment.NewLine + SearchBullet + (char)Type;

            //Now then, let's search for indices with this indent and a new line before this.
            while (!string.IsNullOrWhiteSpace(Text)) {

                GlobalLogger?.Debug($"Finding next bullet. {Text.Length} Characters remaining");

                int NextBulletIndex = Text.IndexOf(SearchBullet);
                if (NextBulletIndex == -1) { NextBulletIndex = Text.Length;}

                GlobalLogger?.Debug($"Found a Bullet, Adding the list item");
                //Get the substring from the current part of the list to the start of the next one. 
                L.Items.Add(BulletListItem.MakeBulletListItem(Text[0..NextBulletIndex],Type, GlobalLogger));
                Text = Text[NextBulletIndex..].TrimStart();

            }

            return L;

        }
    }
}
