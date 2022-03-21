using Microsoft.EntityFrameworkCore;
using Atlas.Common;

namespace Atlas.Data {

    /// <summary>DBContext to get all objects relating to Neco from the DB</summary>
    public class AtlasContext : PostgresContext {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //This Pragma is disabled because all of these dbsets will work once configured which is by the time anything's going to use it :)

        public DbSet<User> User { get; set; }

        public DbSet<Article> Article { get; set; }

        public DbSet<Image> Image { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
