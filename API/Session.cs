namespace Igtampe.ChopoSessionManager {

    /// <summary>Holds a Neco Session</summary>
    public class Session {

        /// <summary>ID of this session</summary>
        public Guid ID { get; }

        /// <summary>Time at which this session will no longer be valid</summary>
        public DateTime ExpirationDate { get; private set; } = DateTime.MinValue;

        /// <summary>User tied to this Session</summary>
        public string UserID { get; }

        /// <summary>Whether or not this session is expired.</summary>
        public bool Expired => DateTime.UtcNow > ExpirationDate;

        /// <summary>Creates a session for the given UserID</summary>
        /// <param name="UserID"></param>
        public Session(string UserID) {
            this.UserID = UserID;
            ID = Guid.NewGuid();
            ExtendSession();
        }

        /// <summary>Extends the Session expiration date to 15 minutes after now</summary>
        public void ExtendSession() {
            if (ExpirationDate != DateTime.MinValue && Expired) { throw new InvalidOperationException("Session is already expired"); }
            ExpirationDate = DateTime.UtcNow.AddHours(12);
        }

        /// <summary>Compares this Session to another object</summary>
        /// <param name="obj"></param>
        /// <returns>True if and only if object is a session and the ID matches</returns>
        public override bool Equals(object? obj) => obj is Session session && ID.Equals(session.ID);

        /// <summary>Gets hashcode for this session</summary>
        /// <returns><see cref="ID"/>'s hashcode</returns>
        public override int GetHashCode() => HashCode.Combine(ID);
    }
}
