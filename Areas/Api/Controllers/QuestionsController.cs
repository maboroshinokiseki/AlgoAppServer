using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class QuestionsController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public QuestionsController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("{qid}")]
        public async Task<QuestionModel> GetQuestionAsync(int qid)
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var role = HttpContext.User.Claims.GetClaim(ClaimTypes.Role);
            var question = await GetQuestionOnlyAsync(qid);

            if (role == UserRole.Teacher.ToString())
            {
                question.AnswerResult = await GetCorrectAnswerAsync(qid, question.Type);
            }
            else if (role == UserRole.Student.ToString())
            {
                var userAnswer = await _dbContext.UserAnswers.OrderBy(a => a.Id).LastOrDefaultAsync(a => a.UserId == uid && a.QuestionId == question.Id);
                if (userAnswer != null)
                {
                    question.AnswerResult = await GetAnswerAsync(qid, question.Type, userAnswer.Id);
                    question.Status = question.AnswerResult.Correct ? QuestionStatus.CorrectAnswer : QuestionStatus.WrongAnswer;
                }
            }

            return question;
        }

        [HttpGet("[action]")]
        public async Task<QuestionModel> DailyPractice()
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var DailyData = await _dbContext.DailyPractices.OrderBy(d => d.Id).LastOrDefaultAsync(d => d.UserId == uid);
            if (DailyData != null)
            {
                if (DailyData.Date != DateTime.Today)
                {
                    _dbContext.DailyPractices.Remove(DailyData);
                    await _dbContext.SaveChangesAsync();
                }
                else if (DailyData.Count >= 10)
                {
                    return new QuestionModel { Code = Codes.NoMoreQuestions };
                }
            }
            var random = new Random();
            //Find not touched question first
            var notTouchedQuestion = _dbContext.Questions.Where(q => !_dbContext.UserAnswers.Any(u => u.UserId == uid && u.QuestionId == q.Id));
            var notTouchedQuestionCount = await notTouchedQuestion.CountAsync();
            int questionId;
            if (notTouchedQuestionCount == 0)
            {
                var todayNotTouchedQuestion = _dbContext.Questions.Where(q => !_dbContext.UserAnswers.Any(u => u.UserId == uid && u.QuestionId == q.Id && u.TimeStamp.Date == DateTime.Today));
                var todayNotTouchedQuestionCount = await todayNotTouchedQuestion.CountAsync();
                if (todayNotTouchedQuestionCount == 0)
                {
                    questionId = await _dbContext.Questions.OrderBy(q => q.Id).Skip(random.Next(0, await _dbContext.Questions.CountAsync())).Take(1).Select(q => q.Id).FirstAsync();
                }
                else
                {
                    var lastAnswer = await _dbContext.UserAnswers.OrderBy(a => a.Id)
                                                                 .LastOrDefaultAsync(a => a.UserId == uid);
                    questionId = await todayNotTouchedQuestion.OrderBy(q => q.Id)
                                                              .Where(q => q.Id != lastAnswer.QuestionId)
                                                              .Skip(random.Next(0, todayNotTouchedQuestionCount - 1))
                                                              .Take(1)
                                                              .Select(q => q.Id)
                                                              .FirstAsync();
                }
            }
            else
            {
                questionId = await notTouchedQuestion.OrderBy(q => q.Id).Skip(random.Next(0, notTouchedQuestionCount)).Take(1).Select(q => q.Id).FirstAsync();
            }
            var question = await GetQuestionOnlyAsync(questionId);

            return ObjectMapper.Map<QuestionModel>(question);
        }

        [HttpGet("[action]")]
        public async Task<QuestionModel> BreakThrough()
        {
            var uid = int.Parse(HttpContext.User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var question = await _dbContext.Questions.Where(q => !_dbContext.UserAnswers.Any(u => u.UserId == uid && u.QuestionId == q.Id))
                                                     .OrderBy(q => q.Difficulty)
                                                     .FirstOrDefaultAsync();
            if (question == null)
            {
                return new QuestionModel { Code = Codes.NoMoreQuestions };
            }
            else
            {
                return ObjectMapper.Map<QuestionModel>(await GetQuestionOnlyAsync(question.Id));
            }
        }

        [HttpGet("{qid}/{aid}")]
        public async Task<QuestionModel> GetQuestionAsync(int qid, int aid)
        {
            var question = await GetQuestionOnlyAsync(qid);
            question.AnswerResult = await GetAnswerAsync(qid, question.Type, aid);
            question.Status = question.AnswerResult.Correct ? QuestionStatus.CorrectAnswer : QuestionStatus.WrongAnswer;

            return question;
        }

        private async Task<QuestionModel> GetQuestionOnlyAsync(int questionId)
        {
            var question = ObjectMapper.Map(await _dbContext.Questions.FindAsync(questionId), new QuestionModel { Status = QuestionStatus.Untouched });

            if (question.Type == QuestionType.SingleSelection)
            {
                var options = await _dbContext.SelectionOptions.Where(a => a.QuestionId == questionId)
                    .Select(a => new QuestionModel.Option { Id = a.Id, Content = a.Content })
                    .ToListAsync();
                question.Options = options;
            }

            return question;
        }

        private async Task<AnswerResultModel> GetCorrectAnswerAsync(int questionId, QuestionType questionType)
        {
            if (questionType == QuestionType.SingleSelection)
            {
                var correctAnswer = await _dbContext.SelectionOptions.FirstAsync(a => a.QuestionId == questionId && a.Correct == true);
                return new AnswerResultModel() { CorrectAnswer = correctAnswer.Content };
            }

            return null;
        }

        private async Task<AnswerResultModel> GetAnswerAsync(int questionId, QuestionType questionType, int userAnswerId)
        {
            var userAnswer = await _dbContext.UserAnswers.FindAsync(userAnswerId);
            if (userAnswer != null)
            {
                if (questionType == QuestionType.SingleSelection)
                {
                    var userAnswerDetail = await _dbContext.SelectionOptions.FindAsync(int.Parse(userAnswer.MyAnswers[0]));
                    var result = await GetCorrectAnswerAsync(questionId, questionType);
                    result.Correct = userAnswer.Correct;
                    result.UserAnswer = userAnswerDetail.Content;
                    return result;
                }
            }

            return null;
        }

        [HttpGet("EasyToGetWrongChapterssByClass/{classId}")]
        public async Task<CommonListResultModel<EasyToGetWrongQuestionModel>> EasyToGetWrongChapterssByClass(int classId)
        {
            var studentIds = _dbContext.StudentsToClasses.Where(sc => sc.ClassRoomId == classId).Select(sc => sc.StudentId);
            var questionIds = _dbContext.UserAnswers.Where(a => studentIds.Contains(a.UserId) && a.Correct == false)
                                                    .Select(a => a.QuestionId)
                                                    .Distinct();
            var chapterIds = await _dbContext.Questions.Where(q => questionIds.Contains(q.Id))
                                                       .Select(q => q.Chapter)
                                                       .Distinct()
                                                       .ToListAsync();
            var result = new CommonListResultModel<EasyToGetWrongQuestionModel> { Items = new List<EasyToGetWrongQuestionModel>() };
            foreach (var item in chapterIds)
            {
                double allAnswerCount = await _dbContext.UserAnswers.Where(a => questionIds.Contains(a.QuestionId) && a.Question.ChapterId == item.Id).CountAsync();
                double incorrectAnswerCount = await _dbContext.UserAnswers.Where(a => questionIds.Contains(a.QuestionId) && a.Question.ChapterId == item.Id && a.Correct == false).CountAsync();
                result.Items.Add(new EasyToGetWrongQuestionModel { ChapterId = item.Id, Content = item.Name, ErrorRatio = incorrectAnswerCount / allAnswerCount });
            }

            return result;
        }

        [HttpGet("EasyToGetWrongQuestionsByClass/{classId}")]
        public async Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestionsByClassAsync(int classId)
        {
            var studentIds = _dbContext.StudentsToClasses.Where(sc => sc.ClassRoomId == classId).Select(sc => sc.StudentId);
            var answers = await _dbContext.UserAnswers.Where(a => studentIds.Contains(a.UserId)).GroupBy(a => a.QuestionId).ToListAsync();
            var result = new CommonListResultModel<EasyToGetWrongQuestionModel> { Items = new List<EasyToGetWrongQuestionModel>() };
            foreach (var item in answers)
            {
                var question = await _dbContext.Questions.FindAsync(item.Key);
                result.Items.Add(new EasyToGetWrongQuestionModel { QuestionId = item.Key, Content = question.Content, ErrorRatio = (double)item.Count(a => a.Correct == false) / (double)item.Count() });
            }

            return result;
        }

        [HttpGet("EasyToGetWrongQuestionsByQuestion/{questionId}")]
        public async Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestionsByQuestionAsync(int questionId)
        {
            var result = new CommonListResultModel<EasyToGetWrongQuestionModel> { Items = new List<EasyToGetWrongQuestionModel>() };
            var question = await _dbContext.Questions.FindAsync(questionId);
            if (question.Type == QuestionType.SingleSelection)
            {
                var answers = await _dbContext.UserAnswers.Include(a => a.User).Where(a => a.QuestionId == questionId && a.Correct == false).ToListAsync();
                foreach (var item in answers)
                {
                    var userOption = await _dbContext.SelectionOptions.FindAsync(int.Parse(item.MyAnswers[0]));
                    result.Items.Add(new EasyToGetWrongQuestionModel { UserNickname = item.User.Nickname, UserAnswer = userOption.Content });
                }
            }

            return result;
        }
    }
}