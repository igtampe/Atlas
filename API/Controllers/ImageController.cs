using Microsoft.AspNetCore.Mvc;
using Atlas.Common;
using Atlas.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;

namespace Atlas.API.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Images")]
    [ApiController]
    public class ImageController : ControllerBase {

        private readonly AtlasContext DB;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        public ImageController(AtlasContext Context) => DB = Context;

        /// <summary>Gets a list of all articles on this Atlas server</summary>
        /// <param name="Query">Search query to search in IDs and </param>
        /// <param name="Take"></param>
        /// <param name="Skip"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Directory([FromQuery] string? Query, [FromQuery] int? Take, [FromQuery] int? Skip) {
            IQueryable<Image> Set = DB.Image;

            if (!string.IsNullOrWhiteSpace(Query)) {
                Query = Query.ToLower();
                Set = Set.Where(A => A.Name.ToLower().Contains(Query));
            }

            Set = Set.OrderByDescending(A => A.DateUploaded).Skip(Skip ?? 0).Take(Take ?? 20);

            return Ok(await Set.ToListAsync());
        }

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetImage([FromRoute]Guid ID) {
            Image? I = await DB.Image.FindAsync(ID);
            return I is null || I.Data is null || I.Type is null ? NotFound("Image was not found") : File(I.Data, I.Type);
        }

        /// <summary>Gets information on an image</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}/info")]
        public async Task<IActionResult> GetImageInfo([FromRoute] Guid ID) {
            Image? I = await DB.Image
                .Include(A => A.Uploader)
                .FirstOrDefaultAsync(A=>A.ID==ID);
            return I is null ? NotFound(ErrorResult.NotFound("Image was not found!")) : Ok(I);
        }

        /// <summary>Uploads an Image to the DB.</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // POST api/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromHeader] Guid SessionID, [FromQuery] string? Name, [FromQuery] string? Description) {

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return Unauthorized(ErrorResult.Reusable.InvalidSession); }

            //Find the user
            User? U = await DB.User.FirstOrDefaultAsync(O=> O.Username == S.UserID);
            if(U is null) { return Unauthorized(ErrorResult.Reusable.InvalidSession); }

            if (!U.IsUploader && !U.IsAdmin) { return Unauthorized(ErrorResult.ForbiddenRoles("Admin or Uploader")); }

            //That is all we will check the session for. In some future other project we may have an `Image Uploader` role to verify but for now this is fine
            string? ContentType = Request.ContentType;

            int MaxSize = 1024 * 1024 * 1;

            if (ContentType != "image/png" && ContentType != "image/jpeg" && ContentType != "image/gif") { 
                return BadRequest(ErrorResult.BadRequest("File must be PNG, JPG, or GIF")); 
            }
            
            if (Request.ContentLength > MaxSize) { 
                return BadRequest(ErrorResult.BadRequest("File must be less than 1mb in size")); 
            }

            Image I = new() { 
                Name = Name ?? "", Description = Description ?? "",
                Type = ContentType 
            };

            using (var memoryStream = new MemoryStream()) {
                await Request.Body.CopyToAsync(memoryStream);
                I.Data = memoryStream.ToArray();
                if (I.Data.Length > MaxSize) { return BadRequest(ErrorResult.BadRequest("File must be less than 1mb in size")); }
            }

            DB.Image.Add(I);
            await DB.SaveChangesAsync();
            return Ok(I);

        }
        
        /// <summary>Deletes an image from the DB</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the image to delete</param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete([FromHeader] Guid SessionID, [FromRoute] Guid ID) {

            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return Unauthorized("Invalid Session"); }

            //Find the user
            User? U = await DB.User.FirstOrDefaultAsync(O => O.Username == S.UserID);
            if (U is null) { return Unauthorized("Invalid Session"); }
            
            var I = await DB.Image.Include(A => A.Uploader).FirstOrDefaultAsync(A => A.ID == ID);
            if (I is null) { return NotFound(ErrorResult.NotFound("Image was not found"));  }

            if (I.Uploader!=U && !U.IsAdmin) { return Unauthorized(ErrorResult.Forbidden("You don't own this image, and you're not an admin")); }

            DB.Image.Remove(I);
            await DB.SaveChangesAsync();

            return Ok();
        }
    }
}
