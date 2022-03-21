using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Atlas.Common {

    /// <summary>An Atlas user</summary>
    public class User {

        /// <summary>Username of this author</summary>
        [Key]
        public string Username { get; set; } = "";

        /// <summary>Name of this user</summary>
        public string Name { get; set; } = "";

        /// <summary>Profile picture of this user</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Password of this User</summary>
        [JsonIgnore]
        public string Password { get; set; } = "";

        //-[Roles]-

        /// <summary>Edit level a user has</summary>
        public int EditLevel { get; set; } = 0;
        
        /// <summary>Whether or not this user is a Neco Admin</summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>Whether or not this user is allowed to upload images to the server</summary>
        public bool IsUploader { get; set; } = false;

        //-[Overrides]-

    }
}
