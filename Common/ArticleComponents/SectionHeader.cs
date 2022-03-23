using Igtampe.BasicLogger;

namespace Atlas.Common.ArticleComponents {
    
    /// <summary>Header of a Section of an Article</summary>
    public class SectionHeader {

        /// <summary>ComponentName for the frontend</summary>
        public string ComponentName => "SECTION";

        /// <summary>Title of this section</summary>
        public string Title { get; set; } = "";

        /// <summary>Level of this Header</summary>
        public int Level { get; set; } = 0;

        /// <summary>Parses text to make a section header</summary>
        /// <param name="Text"></param>
        /// <param name="GlobalLogger"></param>
        /// <returns></returns>
        public static SectionHeader MakeHeader(string Text, Logger? GlobalLogger = null) {

            GlobalLogger?.Debug("Making Header");

            SectionHeader header = new();

            Text = Text.Trim();
            //Count the number of = signs at the beginning

            for (int i = 0; i < Text.Length; i++) {
                if (Text[i] != '=') { header.Level = i; break; }
            }

            header.Title = Text[(header.Level)..];
            GlobalLogger?.Debug($"Found header {header.Title} of level {header.Level}");

            return header;
            
        }
    }
}
