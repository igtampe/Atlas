using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Atlas.Common.ArticleComponents;
using Igtampe.BasicLogger;

namespace Atlas.Common {
    /// <summary>An Atlas Article</summary>
    public class Article {

        /// <summary>Configured Global Logger for all Articles</summary>
        public static Logger? GlobalLogger { get; set; } = null;

        /// <summary>Unique title of this Atlas article</summary>
        [Key]
        public string Title { get; set; } = "";

        /// <summary>private text holder</summary>
        private string text = "";

        /// <summary>Text of this article</summary>
        public string Text {
            get => text; //Retrieve text
            set { text = value; ParseText(); } //Set the text, clear the sections
        }

        /// <summary>Date this article was created</summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>Original Author of this Article</summary>
        public string OriginalAuthor { get; set; } = "";

        /// <summary>Date this article was last updated</summary>
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>Author that last updated this Article</summary>
        public string LastAuthor { get; set; } = "";

        /// <summary>Required edit level of this Article</summary>
        public int EditLevel { get; set; } = 0;

        /// <summary>The sidebar of this article</summary>
        [NotMapped]
        public Section? Sidebar { get; set; } = null;

        /// <summary>List of atlas sections in this article</summary>
        [NotMapped]
        public Section? MainSection { get; set; } = null;

        /// <summary>Checks if a User can edit this article</summary>
        /// <param name="U"></param>
        /// <returns></returns>
        public bool CanEdit(User U) => U.EditLevel >= EditLevel || U.IsAdmin;

        /// <summary>Converts Atlas format text into Atlas Sections</summary>
        /// <returns></returns>
        private void ParseText() {

            GlobalLogger?.Debug($"Beginning to parse new text for article {Title}");

            Sidebar = null;
            MainSection = null;

            //Get the Sidebar
            Match SidebarMatch = Regex.Match(Text, ">.*\r\n(.*\r\n)*>");
            if (SidebarMatch.Success) {

                GlobalLogger?.Debug($"Found a Sidebar");

                //We've got the sidebar!
                Sidebar = Section.MakeSection(SidebarMatch.Value[1..(SidebarMatch.Value.Length-3)],-1,GlobalLogger);
                Text = Text.Replace(SidebarMatch.Value, "");

            }

            GlobalLogger?.Debug($"Parsing Main Section");
            MainSection = Section.MakeSection(Text,0,GlobalLogger);
            GlobalLogger?.Debug($"Wrapping up...");

        }
    }
}