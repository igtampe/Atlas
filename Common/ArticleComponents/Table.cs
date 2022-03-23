using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents {

    /// <summary>A Table with an optional title and header</summary>
    public class Table {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "TABLE";

        /// <summary>A Row of a Table</summary>
        public class Row {

            /// <summary>An individual cell of a row of a table</summary>
            public class Cell {

                /// <summary>Text of this Cell</summary>
                public List<FormattedText> Text { get; set; }

                /// <summary>Creates a cell</summary>
                /// <param name="CellText"></param>
                public Cell(string CellText) => Text = FormattedText.FormatText(CellText);

            }

            /// <summary>Cells in this row</summary>
            public List<Cell> Cells { get; set; } = new();

            /// <summary>Width in Columns</summary>
            public int Width => Cells.Count;

        }

        /// <summary>Title of this Table</summary>
        public string? Title { get; set; } = null;
        
        /// <summary>Header row</summary>
        public Row? HeaderRow { get; set; } = null;

        /// <summary>Additional Rows</summary>
        public List<Row> Rows { get; } = new();

        /// <summary>Height of this table in Rows</summary>
        public int Height => Rows.Count;

        /// <summary>Maximum width in columns</summary>
        public int Width => Rows.Max(A => A.Width);

        /// <summary>Constructs a table from a given </summary>
        /// <param name="Text"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static Table MakeTable(string Text, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug($"Creating a Table");

            Table T = new();

            string[] RowTexts = Text.Split(Environment.NewLine);
            if (RowTexts.Length < 3) {
                GlobalLogger?.Error($"Table did not have at least 3 lines. Generated an ErrorTable");
                return ErrorTable("Table needs at least 3 lines. Title (even if blank), Header Row (even if blank), and at least one row of actual data");
            }

            //Title
            if (RowTexts[0].Length > 2) { T.Title = RowTexts[0][2..]; }

            //header and other rows
            GlobalLogger?.Debug($"Processing Header Row");
            if (!string.IsNullOrWhiteSpace(RowTexts[1].Replace("|", ""))) { T.HeaderRow = MakeRow(RowTexts[1]); }

            GlobalLogger?.Debug($"Processing Rows");
            foreach (string RowText in RowTexts[2..]) { if (!string.IsNullOrWhiteSpace(RowText)) { T.Rows.Add(MakeRow(RowText)); } }

            GlobalLogger?.Debug($"Done, Adios");

            return T;
        }

        private static Row MakeRow(string Text) {
            Row R = new();
            //Trim the start of the thing
            Text = Text[1..];

            // Split the text
            string[] CellTexts = Text.Split('|');
            foreach (string CellText in CellTexts[..(CellTexts.Length-1)]) { R.Cells.Add(new(CellText.TrimStart().TrimEnd())); }
            
            return R;
        }

        private static Table ErrorTable(string Error) {

            Table T = new();

            T.Title = "Error";
            T.Rows.Add(MakeRow($"| {Error} |"));

            return T;

        }
    }
}
