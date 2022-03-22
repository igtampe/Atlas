using Microsoft.AspNetCore.Mvc;
using Atlas.Common;
using Atlas.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;

namespace Atlas.API.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Article")]
    [ApiController]
    public class ArticleController : ControllerBase {

        private readonly AtlasContext DB;
        private static readonly User AnonymousUser = new() {
            Username = "Anonymous",  EditLevel = 0,
            IsAdmin = false, IsUploader = false,
        };

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        public ArticleController(AtlasContext Context) => DB = Context;

        #region Gets
        /// <summary>Gets a list of all articles on this Atlas server</summary>
        /// <param name="Query">Search query to search in IDs and </param>
        /// <param name="Take"></param>
        /// <param name="Skip"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Directory([FromQuery] string? Query, [FromQuery] int? Take, [FromQuery] int? Skip) {
            IQueryable<Article> Set = DB.Article;
            
            if (!string.IsNullOrWhiteSpace(Query)) {
                Query = Query.ToLower();
                Set = Set.Where(A => A.Title.ToLower().Contains(Query)); 
            }

            Set = Set.OrderByDescending(A=>A.DateUpdated).Skip(Skip ?? 0).Take(Take ?? 20);

            return Ok(await Set.ToListAsync());
        }

        /// <summary>Gets a given Article</summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("{Title}")]
        public async Task<IActionResult> GetArticleEntry(string Title) {
            //Get the Article
            Title = Title.ToLower();
            Article? R = await DB.Article.FirstOrDefaultAsync(A=>A.Title.ToLower()==Title);
            return R is null ? NotFound(ErrorResult.NotFound("Article was not found!","Title")) : Ok(R);
        }

        #endregion

        #region Puts

        /// <summary>Handles updating Articles</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Text">New text for this article</param>
        /// <param name="Title">Title of the article to edit</param>
        /// <returns></returns>
        // PUT API/Article/{ID}
        [HttpPut("{Title}")]
        public async Task<IActionResult> Update([FromHeader] Guid? SessionID, [FromRoute] string Title,[FromBody] string Text) {

            var E = await GetEditor(SessionID);
            if (E.Item2 is not null) { return E.Item2; }
            User Editor = E.Item1;

            //Get the article
            Article? R = await GetArticle(Title);
            if (R is null) { return NotFound(ErrorResult.NotFound("Article was not found!","Title")); }
            if (!R.CanEdit(Editor)) { return Unauthorized(ErrorResult.Forbidden("You do not have enough edit level to edit this article", "User")); }

            //OK now then
            R.Text=Text;

            R.DateUpdated = DateTime.UtcNow;
            R.LastAuthor = Editor.Username;

            DB.Update(R);

            await DB.SaveChangesAsync();

            return Ok(R);

        }

        /// <summary>Request to change an Article's Editing level</summary>
        /// <param name="SessionID"></param>
        /// <param name="Title"></param>
        /// <param name="NewEditLevel"></param>
        /// <returns></returns>
        [HttpPut("{Title}/EditLevel")]
        public async Task<IActionResult> UpdateEditLevel([FromHeader] Guid? SessionID, [FromRoute] string Title, [FromQuery] int NewEditLevel) {

            var E = await GetEditor(SessionID);
            if (E.Item2 is not null) { return E.Item2; }
            User Editor = E.Item1;

            if (NewEditLevel > Editor.EditLevel && !Editor.IsAdmin) { return Unauthorized(ErrorResult.Forbidden("Cannot lock yourself out of this article")); }

            //Get the article
            Article? R = await GetArticle(Title);
            if (R is null) { return NotFound(ErrorResult.NotFound("Article was not found!", "Title")); }
            if (!R.CanEdit(Editor)) { return Unauthorized(ErrorResult.Forbidden("You do not have enough edit level to edit this article", "User")); }
            
            await DB.SaveChangesAsync();

            return Ok(R);

        }

        #endregion

        #region Posts

        // POST api/Users
        /// <summary>Handles Creating articles, and optionally saving them to the DB</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Text">Text of this new article</param>
        /// <param name="Title">Title of this new article</param>
        /// <param name="Save">Whether or not to save this article to the DB</param>
        /// <returns></returns>
        [HttpPost("{Title}")]
        public async Task<IActionResult> Create([FromHeader] Guid? SessionID, [FromRoute] string Title, [FromBody] string Text, [FromQuery] bool? Save) {

            var E = await GetEditor(SessionID);
            if (E.Item2 is not null) { return E.Item2; }
            User Editor = E.Item1;

            if (GetArticle(Title) is not null) { return BadRequest(ErrorResult.BadRequest("An article with this title already exists!")); }

            Article R = new() {
                Title = Title, Text = Text,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                OriginalAuthor = Editor.Username,
                LastAuthor = Editor.Username,
                EditLevel = 0,
            };

            if (Save ?? false) {
                DB.Add(R);
                await DB.SaveChangesAsync();
            }

            //OK adios
            return Ok(R);

        }

        #endregion

        #region Deletes

        /// <summary>Deletes an article from the DB</summary>
        /// <param name="SessionID"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete([FromHeader] Guid? SessionID, [FromRoute] string Title) {

            var E = await GetEditor(SessionID);
            if (E.Item2 is not null) { return E.Item2; }
            User Editor = E.Item1;

            if (Editor.Equals(AnonymousUser)) { return Unauthorized(ErrorResult.Forbidden("Anonymous users cannot delete articles")); }

            Article? R = await GetArticle(Title);
            if (R is null) { return NotFound(ErrorResult.NotFound("Article was not found!", "Title")); }
            if (!R.CanEdit(Editor)) { return Unauthorized(ErrorResult.Forbidden("You do not have enough edit level to edit this article", "User")); }

            DB.Remove(R);
            await DB.SaveChangesAsync();

            return Ok(R);

        }

        #endregion

        #region Helpers

        private async Task<(User,IActionResult?)> GetEditor(Guid? SessionID) {
            User Editor = AnonymousUser;
            if (SessionID is not null) {
                Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
                if (S is null) { return (Editor, Unauthorized(ErrorResult.Reusable.InvalidSession)); }

                User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID);
                if (U is null) { return (Editor,Unauthorized(ErrorResult.Reusable.InvalidSession)); }

                Editor = U;
            }
            return (Editor,null);
        }

        private async Task<Article?> GetArticle(string Title) {
            Title = Title.ToLower();
            return await DB.Article.FirstOrDefaultAsync(A => A.Title.ToLower() == Title);
        }

        #endregion
    }
}
