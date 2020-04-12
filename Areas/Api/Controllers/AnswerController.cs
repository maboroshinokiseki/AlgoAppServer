using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
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
            public bool IsDailyPractice { get; set; }
        }

        [HttpPost]
        public async Task<AnswerResultModel> OnPostAsync([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _dbContext.Questions.FindAsync(model.QuestionId);
                var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
                if (question.Type == QuestionType.SingleSelection)
                {
                    var answer = await _dbContext.SelectionOptions.FindAsync(model.AnswerId);
                    await _dbContext.UserAnswers.AddAsync(new UserAnswer
                    {
                        UserId = uid,
                        QuestionId = model.QuestionId,
                        Correct = answer.Correct,
                        MyAnswers = new List<string>() { model.AnswerId.ToString() },
                        TimeStamp = DateTime.UtcNow,
                    });

                    var result = new AnswerResultModel() { Correct = answer.Correct, UserAnswer = answer.Content, CorrectAnswer = answer.Content };
                    if (!answer.Correct)
                    {
                        var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == model.QuestionId && a.Correct == true);
                        result.CorrectAnswer = correctAnswer.Content;
                    }
                    else
                    {
                        var u = await _dbContext.Users.FindAsync(uid);
                        u.Points += question.Difficulty + 1;
                        //Process daily points
                        var DailyData = await _dbContext.DailyPoints.OrderBy(d => d.Date).LastOrDefaultAsync(d => d.UserId == uid);
                        if (DailyData != null)
                        {
                            if (DailyData.Date == DateTime.Today)
                            {
                                DailyData.Points += question.Difficulty + 1;
                            }
                            else
                            {
                                if (DailyData.Date == DateTime.Today.AddDays(-1))
                                {
                                    await _dbContext.DailyPoints.AddAsync(new DailyPoints { UserId = uid, Date = DateTime.Today, Points = question.Difficulty + 1 });
                                }
                                else
                                {
                                    _dbContext.DailyPoints.Remove(DailyData);
                                }
                            }
                        }
                        else
                        {
                            await _dbContext.DailyPoints.AddAsync(new DailyPoints { UserId = uid, Date = DateTime.Today, Points = question.Difficulty + 1 });
                        }
                    }

                    if (model.IsDailyPractice)
                    {
                        var DailyData = await _dbContext.DailyPractices.OrderBy(d => d.Date).LastOrDefaultAsync(d => d.UserId == uid);
                        if (DailyData != null)
                        {
                            if (DailyData.Date == DateTime.Today)
                            {
                                DailyData.Count++;
                            }
                            else
                            {
                                _dbContext.DailyPractices.Remove(DailyData);
                            }
                        }
                        else
                        {
                            await _dbContext.DailyPractices.AddAsync(new DailyPractice { UserId = uid, Date = DateTime.Today, Count = 1 });
                        }
                    }

                    await _dbContext.SaveChangesAsync();

                    return result;
                }
            }

            return new AnswerResultModel { Code = Codes.Unknown, Description = "Unknow Error." };
        }

        [HttpGet("{uid}/historyQuestions/{cid}")]
        public async Task<CommonListResultModel<HistoryItemModel>> GetAnswerHistoryQuestionsAsync(int uid, int cid)
        {
            var answers = await _dbContext.UserAnswers.Include(a => a.Question)
                                                      .Where(a => a.UserId == uid && a.Question.ChapterId == cid)
                                                      .OrderByDescending(a => a.TimeStamp)
                                                      .ToListAsync();

            var histories = new List<HistoryItemModel>();
            foreach (var answer in answers)
            {
                if (answer.Question.Type == QuestionType.SingleSelection)
                {
                    histories.Add(new HistoryItemModel() { QuestionId = answer.QuestionId, AnswerId = answer.Id, QuestionContent = answer.Question.Content, Correct = answer.Correct });
                }
            }

            return new CommonListResultModel<HistoryItemModel> { Items = histories };
        }

        [HttpGet("{uid}/historyChapters")]
        public async Task<CommonListResultModel<Chapter>> GetAnswerHistoryChaptersAsync(int uid)
        {
            var questionIds = _dbContext.UserAnswers.Where(a => a.UserId == uid)
                                                    .GroupBy(a => a.QuestionId)
                                                    .Select(a => a.Key);
            var chapters = await _dbContext.Questions.Where(q => questionIds.Contains(q.Id))
                                                     .Select(q => q.Chapter)
                                                     .Distinct()
                                                     .ToListAsync();

            return new CommonListResultModel<Chapter> { Code = Codes.None, Items = chapters };
        }
    }
}