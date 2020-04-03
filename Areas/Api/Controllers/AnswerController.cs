using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnswerController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public AnswerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public class PostModel
        {
            public int QuestionId { get; set; }

            public int AnswerId { get; set; }
        }

        [HttpPost]
        public async Task<AnswerResultModel> OnPostAsync([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _dbContext.Questions.FindAsync(model.QuestionId);
                if (question.Type == QuestionType.SingleSelection)
                {
                    var uid = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    var answer = await _dbContext.SelectionOptions.FindAsync(model.AnswerId);
                    await _dbContext.UserAnswers.AddAsync(new UserAnswer
                    {
                        UserId = int.Parse(uid),
                        QuestionId = model.QuestionId,
                        Correct = answer.Correct,
                        MyAnswers = new List<string>() { model.AnswerId.ToString() },
                        TimeStamp = DateTime.UtcNow,
                    });

                    await _dbContext.SaveChangesAsync();

                    var result = new AnswerResultModel() { Correct = answer.Correct, UserAnswer = answer.Content, CorrectAnswer = answer.Content, Analysis = question.Analysis };
                    if (!answer.Correct)
                    {
                        var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == model.QuestionId && a.Correct == true);
                        result.CorrectAnswer = correctAnswer.Content;
                    }
                    else
                    {
                        var u = await _dbContext.Users.FindAsync(int.Parse(uid));
                        u.Points += question.Difficulty + 1;
                    }

                    return result;
                }
            }

            return new AnswerResultModel { Code = Codes.Unknown, Description = "Unknow Error." };
        }

        [HttpGet("histories/{uid}")]
        public async Task<CommonListResultModel<HistoryItemModel>> GetAnswerHistoriesAsync(int uid)
        {
            var answers = await _dbContext.UserAnswers
            .Include(a => a.Question)
            .Where(a => a.UserId == uid)
            .OrderByDescending(a => a.TimeStamp)
            .ToListAsync();

            var histories = new List<HistoryItemModel>();
            foreach (var answer in answers)
            {
                if (answer.Question.Type == QuestionType.SingleSelection)
                {
                    histories.Add(new HistoryItemModel() { QuestionId = answer.QuestionId,AnswerId = answer.Id, QuestionContent = answer.Question.Content, Correct = answer.Correct });
                }
            }

            return new CommonListResultModel<HistoryItemModel> { Items = histories };
        }
    }
}