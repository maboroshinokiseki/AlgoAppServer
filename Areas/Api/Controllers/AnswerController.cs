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

                    return result;
                }
            }

            return new AnswerResultModel { Code = Codes.Unknown, Description = "Unknow Error." };
        }

        public class HistoryItemModel
        {
            public int AnswerId { get; set; }
            public int QuestionId { get; set; }
            public string QuestionContent { get; set; }
            public bool Correct { get; set; }
        }
        [HttpGet("histories")]
        public async Task<List<HistoryItemModel>> GetAnswerHistoriesAsync()
        {
            var nameIdentifier = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var uid = int.Parse(nameIdentifier);
            var answers = await _dbContext.UserAnswers
            .Include(a => a.Question)
            .Where(a => a.UserId == uid)
            .OrderByDescending(a => a.TimeStamp)
            .ToListAsync();

            var histories = new List<HistoryItemModel>();
            foreach (var answer in answers)
            {
                if (answer.Question.Type == QuestionType.SingleSelection || answer.Question.Type == QuestionType.SingleSelection)
                {
                    histories.Add(new HistoryItemModel() { QuestionContent = answer.Question.Content, Correct = answer.Correct });
                }
            }

            return histories;
        }

        // [HttpGet("histories/{aid}")]
        // public async Task<List<HistoryItemModel>> GetAnswerHistoryAsync(int aid)
        // {
        //     var answers = await _dbContext.Answers
        //     .Include(a => a.Question)
        //     .FirstOrDefaultAsync();

        //     var histories = new List<HistoryItemModel>();
        //     foreach (var answer in answers)
        //     {
        //         if (answer.Question.Type == QuestionType.SingleSelection || answer.Question.Type == QuestionType.SingleSelection)
        //         {
        //             histories.Add(new HistoryItemModel() { QuestionTitle = answer.Question.Title, Correct = answer.Correct });
        //         }
        //     }

        //     return histories;
        // }
    }
}