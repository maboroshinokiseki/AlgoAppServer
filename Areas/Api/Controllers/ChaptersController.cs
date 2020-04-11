using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChaptersController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public ChaptersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<CommonListResultModel<Chapter>> GetChaptersAsync()
        {
            return new CommonListResultModel<Chapter> { Code = Codes.None, Items = await _dbContext.Chapters.OrderBy(c => c.Order).ToListAsync() };
        }

        [HttpGet("{cid}/questions")]
        public async Task<CommonListResultModel<QuestionModel>> GetQuestionsAsync(int cid)
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var role = HttpContext.User.Claims.GetClaim(ClaimTypes.Role);
            var items = await _dbContext.Questions.Where(q => q.ChapterId == cid).Select(q => new QuestionModel { Id = q.Id, Content = q.Content }).ToListAsync();
            if (role == UserRole.Student.ToString())
            {
                foreach (var item in items)
                {
                    var userAnswer = await _dbContext.UserAnswers.OrderBy(a => a.Id).LastOrDefaultAsync(a => a.UserId == uid && a.QuestionId == item.Id);
                    if (userAnswer != null)
                    {
                        item.Status = userAnswer.Correct ? QuestionStatus.CorrectAnswer : QuestionStatus.WrongAnswer;
                    }
                }
            }
            return new CommonListResultModel<QuestionModel> { Code = Codes.None, Items = items };
        }
    }
}