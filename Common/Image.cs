using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Atlas.Common {

    /// <summary>Image stored on the database</summary>
    public class Image {

        /// <summary>ID of this image</summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; } = Guid.Empty;

        /// <summary>Person who uploaded this image</summary>
        public User? Uploader { get; set; }

        /// <summary>Name of this image</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this image</summary>
        public string Description { get; set; } = "";

        /// <summary>Data of this image</summary>
        [JsonIgnore]
        public byte[]? Data { get; set; }

        /// <summary>MIME Type of this image (image/png)</summary>
        public string Type { get; set; } = "";

        /// <summary>Gives basic information of this Image</summary>
        /// <returns></returns>
        public override string ToString() => $"Image \'{ID}\' ({Type})";
    }
}
