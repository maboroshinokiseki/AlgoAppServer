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
            public List<int> AnswerIds { get; set; }
            public bool IsDailyPractice { get; set; }
        }

        [HttpPost]
        public async Task<AnswerResultModel> OnPostAsync([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _dbContext.Questions.FindAsync(model.QuestionId);
                var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
                var result = new AnswerResultModel();
                if (question.Type == QuestionType.SingleSelection)
                {
                    var answer = await _dbContext.SelectionOptions.FindAsync(model.AnswerIds[0]);
                    await _dbContext.UserAnswers.AddAsync(new UserAnswer
                    {
                        UserId = uid,
                        QuestionId = model.QuestionId,
                        Correct = answer.Correct,
                        MyAnswers = model.AnswerIds.ConvertAll(id => id.ToString()),
                        TimeStamp = DateTime.UtcNow,
                    });
                    result.Correct = answer.Correct;
                    result.UserAnswers = new List<string>() { answer.Content };
                    result.CorrectAnswers = result.UserAnswers;
                    if (!answer.Correct)
                    {
                        var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == model.QuestionId && a.Correct == true);
                        result.CorrectAnswers = new List<string>() { correctAnswer.Content };
                    }
                }
                else if (question.Type == QuestionType.MultiSelection)
                {
                    var userAnswers = await _dbContext.SelectionOptions.Where(o => model.AnswerIds.Contains(o.Id)).ToListAsync();
                    var correctAnswers = await _dbContext.SelectionOptions.Where(o => o.QuestionId == userAnswers[0].QuestionId && o.Correct == true).ToListAsync();
                    var isCorrect = correctAnswers.All(o => userAnswers.Contains(o)) && correctAnswers.Count == userAnswers.Count;
                    await _dbContext.UserAnswers.AddAsync(new UserAnswer
                    {
                        UserId = uid,
                        QuestionId = model.QuestionId,
                        Correct = isCorrect,
                        MyAnswers = model.AnswerIds.ConvertAll(id => id.ToString()),
                        TimeStamp = DateTime.UtcNow,
                    });
                    result.Correct = isCorrect;
                    result.UserAnswers = userAnswers.Select(a => a.Content).ToList();
                    result.CorrectAnswers = correctAnswers.Select(a => a.Content).ToList();
                }

                if (result.Correct)
                {
                    var u = await _dbContext.Users.FindAsync(uid);
                    u.Points += question.Difficulty + 1;
                    //Process daily points
                    var DailyData = await _dbContext.DailyPoints.OrderBy(d => d.Date).LastOrDefaultAsync(d => d.UserId == uid);
                    if (DailyData?.Date == DateTime.Today)
                    {
                        DailyData.Points += question.Difficulty + 1;
                    }
                    else
                    {
                        _dbContext.DailyPoints.RemoveRange(await _dbContext.DailyPoints.Where(d => d.UserId == uid && d.Date < DateTime.Today.AddDays(-1)).ToListAsync());
                        await _dbContext.DailyPoints.AddAsync(new DailyPoints { UserId = uid, Date = DateTime.Today, Points = question.Difficulty + 1 });
                    }
                }

                if (model.IsDailyPractice)
                {
                    var DailyData = await _dbContext.DailyPractices.OrderBy(d => d.Date).LastOrDefaultAsync(d => d.UserId == uid);
                    if (DailyData?.Date == DateTime.Today)
                    {
                        DailyData.Count++;
                    }
                    else
                    {
                        _dbContext.DailyPractices.RemoveRange(await _dbContext.DailyPractices.Where(d => d.UserId == uid && d.Date < DateTime.Today).ToListAsync());
                        await _dbContext.DailyPractices.AddAsync(new DailyPractice { UserId = uid, Date = DateTime.Today, Count = 1 });
                    }
                }

                await _dbContext.SaveChangesAsync();

                return result;
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
                histories.Add(new HistoryItemModel() { QuestionId = answer.QuestionId, AnswerId = answer.Id, QuestionContent = answer.Question.Content, Correct = answer.Correct });
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
            foreach (var item in chapters)
            {
                item.Name = $"第{item.Order}章 {item.Name}";
            }

            return new CommonListResultModel<Chapter> { Code = Codes.None, Items = chapters };
        }
    }
}