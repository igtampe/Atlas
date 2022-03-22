namespace Atlas.API.Requests {

    /// <summary>Request to change a user's roles</summary>
    public class ChangeRoleRequest {

        /// <summary>Whether or not this user is an Administrator</summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>User's edit level</summary>
        public int EditLevel { get; set; } = 0;
        
        /// <summary>Whether or not this user is allowed to upload images to the database</summary>
        public bool IsUploader { get; set; } = false;

    }
}
