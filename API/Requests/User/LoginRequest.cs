namespace Atlas.API.Requests {

    /// <summary>Request to log in to Neco </summary>
    public class LoginRequest {

        /// <summary>Username of this user</summary>
        public string Username { get; set; } = "";

        /// <summary>Password of the user</summary>
        public string Password { get; set; } = "";

    }
}
