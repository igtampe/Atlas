using Microsoft.AspNetCore.Mvc;
using Atlas.Common;
using Atlas.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Atlas.API.Requests;

namespace Atlas.API.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Users")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly AtlasContext DB;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        public UserController(AtlasContext Context) => DB = Context;

        #region Gets
        /// <summary>Gets a directory of this Neco server</summary>
        /// <param name="Query">Search query to search in IDs and </param>
        /// <param name="Take"></param>
        /// <param name="Skip"></param>
        /// <returns></returns>
        [HttpGet("Dir")]
        public async Task<IActionResult> Directory([FromQuery] string? Query, [FromQuery] int? Take, [FromQuery] int? Skip) {
            IQueryable<User> Set = DB.User;
            if (!string.IsNullOrWhiteSpace(Query)) { Set = Set.Where(U => U.Username != null && U.Username.Contains(Query)); }
            Set = Set.Skip(Skip ?? 0).Take(Take ?? 20);

            return Ok(await Set.ToListAsync());
        }

        /// <summary>Gets username of the currently logged in session</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedIn([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return NotFound(ErrorResult.NotFound("Session was not found")); }

            //Get the user
            return await GetUser(S.UserID);
        }

        /// <summary>Gets a given user</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetUser(string ID) {
            //Get the user
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == ID);
            return U is null ? NotFound(ErrorResult.NotFound("User was not found")) : Ok(U);
        }

        #endregion

        #region Puts

        /// <summary>Handles user password changes</summary>
        /// <param name="Request">Request with their current and new password</param>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // PUT api/Users
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] Guid? SessionID, [FromBody] ChangePasswordRequest Request) {

            //Ensure nothing is null
            if (Request.New is null) { return BadRequest("Cannot have empty password"); }

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return Unauthorized("Invalid session"); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID && U.Password == Request.Current);
            if (U is null) { return BadRequest("Incorrect current password"); }

            U.Password = Request.New;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok();

        }

        /// <summary>Request to change a user's roles</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Roles")]
        public async Task<IActionResult> UpdateRoles([FromHeader] Guid? SessionID, [FromRoute] string ID, [FromBody] ChangeRoleRequest Request) {

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return Unauthorized(ErrorResult.Reusable.InvalidSession); }

            //Get Users
            if(!DB.User.Any(U => U.Username == S.UserID && U.IsAdmin)) {
                return Unauthorized(ErrorResult.ForbiddenRoles("Admin"));
            }

            if (S.UserID == ID && !Request.IsAdmin) { return BadRequest(ErrorResult.BadRequest("Cannot un-admin yourself!")); }

            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == ID);
            if (U is null) { return NotFound("User was not found"); }

            //If the user is an admin, and is requested not to be an admin, and is the ONE remaining admin in the entire server
            if (U.IsAdmin && !Request.IsAdmin && 
                (await DB.User.CountAsync(U => U.IsAdmin)) == 1) { return BadRequest(ErrorResult.BadRequest("Cannot remove only admin")); } //Don't

            U.IsAdmin = Request.IsAdmin;
            U.IsUploader = Request.IsUploader;
            U.EditLevel = Request.EditLevel;

            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok(U);

        }

        /// <summary>Request to reset the password of a user</summary>
        /// <param name="SessionID">SessionID of an administrator who wishes to change the password of another user</param>
        /// <param name="ID">ID of the user to change the password of</param>
        /// <param name="Request">Request to change</param>
        /// <returns></returns>
        [HttpPut("{ID}/Reset")]
        public async Task<IActionResult> ResetPassword([FromHeader] Guid? SessionID, [FromRoute] string ID, [FromBody] ChangePasswordRequest Request) {
            //Ensure nothing is null
            if (Request.New is null) { return BadRequest("Cannot have empty password"); }

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return Unauthorized("Invalid session"); }

            //Get Users
            User? Executor = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID);
            if (Executor is null) { return Unauthorized("Invalid Session"); }
            if (!Executor.IsAdmin) { return Unauthorized(ErrorResult.ForbiddenRoles("Admin")); }

            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == ID);
            if (U is null) { return NotFound("User was not found"); }

            U.Password = Request.New;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok();

        }

        /// <summary>Updates the image of the user with this session</summary>
        /// <param name="SessionID"></param>
        /// <param name="ImageURL"></param>
        /// <returns></returns>
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage([FromHeader] Guid? SessionID, [FromBody] string ImageURL) {
            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            if (S is null) { return Unauthorized("Invalid session"); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID);
            if (U is null) { throw new InvalidOperationException("Somehow we're here and we're not supposed to be here"); }

            U.ImageURL = ImageURL;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region Posts

        // POST api/Users
        /// <summary>Handles logging in to Clothespin</summary>
        /// <param name="Request">Request with a User and Password attempt to log in</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginRequest Request) {
            if (Request.Username is null || Request.Password is null) { return BadRequest("ID or Password is null"); }

            //Check the user on the DB instead of the user de-esta cosa
            bool Login = await DB.User.AnyAsync(U => U.Username == Request.Username && U.Password == Request.Password);
            if (!Login) { return Ok("ID or Password is incorrect"); }

            //Generate a session
            return Ok(SessionManager.Manager.LogIn(Request.Username));

        }

        /// <summary>Handles user registration</summary>
        /// <param name="Request">User and password combination to create</param>
        /// <returns></returns>
        // POST api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest Request) {
            if (string.IsNullOrWhiteSpace(Request.Username)) { return BadRequest(ErrorResult.BadRequest("Name cannot be empty", "Name")); }
            if (string.IsNullOrWhiteSpace(Request.Password)) { return BadRequest(ErrorResult.BadRequest("Password cannot be empty", "Password")); }
            if (await DB.User.AnyAsync(A => A.Username == Request.Username)) { return BadRequest(ErrorResult.BadRequest("Username already taken", "Username")); }

            User NewUser = new() {
                Username = Request.Username,
                Password = Request.Password,
            };

            if (!await DB.User.AnyAsync()) { NewUser.IsAdmin = true; } //This is the first account and *MUST* be an admin

            DB.User.Add(NewUser);
            await DB.SaveChangesAsync();

            return Ok(NewUser);

        }

        /// <summary>Handles user logout</summary>
        /// <param name="SessionID">Session to log out of</param>
        /// <returns></returns>
        // POST api/Users/out
        [HttpPost("out")]
        public async Task<IActionResult> LogOut([FromBody] Guid SessionID) => Ok(await Task.Run(() => SessionManager.Manager.LogOut(SessionID)));

        /// <summary>Handles user logout of *all* sessions</summary>
        /// <param name="SessionID">Session that wants to log out of all tied sessions</param>
        /// <returns></returns>
        // POST api/Users/outall
        [HttpPost("outall")]
        public async Task<IActionResult> LogOutAll([FromBody] Guid SessionID) {
            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            return S is null ? Unauthorized("Invalid session") : Ok(await Task.Run(() => SessionManager.Manager.LogOutAll(S.UserID)));
        }

        #endregion
    }
}
