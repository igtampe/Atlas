namespace Atlas.Common.ArticleComponents.SectionComponents {

    /// <summary>An Image Box that shows an image with alt text, and a little description below it</summary>
    public abstract class ImageBox {

        /// <summary>URL to the image</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Alt Text to display for the image</summary>
        public string AltText { get; set; } = "";

        /// <summary>Description of this image</summary>
        public string Description { get; set; } = "";

    }
}
