using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookmarkController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BookmarkController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{questionId}")]
        public async Task<CommonResultModel> IsQuestionInBookmark(int questionId)
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var bookmark = await _dbContext.Bookmarks.FirstOrDefaultAsync(b => b.QuestionId == questionId && b.UserId == uid);
            return new CommonResultModel { Code = bookmark == null ? Codes.QuestionNotInBookmark : Codes.QuestionInBookmark };
        }

        public class AddQuestionToBookmarkModel
        {
            public int QuestionId { get; set; }
        }
        [HttpPost]
        public async Task<CommonResultModel> AddQuestionToBookmark([FromBody] AddQuestionToBookmarkModel model)
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            await _dbContext.Bookmarks.AddAsync(new Bookmark { QuestionId = model.QuestionId, UserId = uid });
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }

        [HttpDelete("{questionId}")]
        public async Task<CommonResultModel> RemoveQuestionFromBookmark(int questionId)
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var bookmark = await _dbContext.Bookmarks.FirstOrDefaultAsync(b => b.QuestionId == questionId && b.UserId == uid);
            if (bookmark ==null)
            {
                return new CommonResultModel { Code = Codes.NoRecord };
            }
            _dbContext.Bookmarks.Remove(bookmark);
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }

        public async Task<CommonListResultModel<QuestionModel>> Questions()
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var questions = await _dbContext.Bookmarks.Where(b => b.UserId == uid)
                                                      .Select(b => b.Question)
                                                      .Select(q => new QuestionModel { Id = q.Id, Content = q.Content })
                                                      .ToListAsync();
            return new CommonListResultModel<QuestionModel> { Code = Codes.None, Items = questions };
        }
    }
}