using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var questionTemp = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == qid);
            var question = new QuestionModel
            {
                Id = questionTemp.Id,
                Content = questionTemp.Content,
                Type = questionTemp.Type,
                Status = QuestionStatus.Untouched,
            };

            if (question.Type == QuestionType.SingleSelection)
            {
                var options = await _dbContext.SelectionOptions.Where(a => a.QuestionId == qid)
                    .Select(a => new QuestionModel.Option { Id = a.Id, Content = a.Content })
                    .ToListAsync();
                question.Options = options;
            }

            if (role == UserRole.Teacher.ToString())
            {
                if (question.Type == QuestionType.SingleSelection)
                {
                    var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == question.Id && a.Correct == true);
                    question.AnswerResult = new AnswerResultModel() { CorrectAnswer = correctAnswer.Content, Analysis = questionTemp.Analysis };
                }
            }
            else if (role == UserRole.Student.ToString())
            {
                var userAnswer = await _dbContext.UserAnswers.OrderBy(a => a.Id).LastOrDefaultAsync(a => a.UserId == uid && a.QuestionId == question.Id);
                if (userAnswer != null)
                {
                    question.Status = userAnswer.Correct ? QuestionStatus.CorrectAnswer : QuestionStatus.WrongAnswer;
                    if (question.Type == QuestionType.SingleSelection)
                    {
                        var userAnswerDetail = await _dbContext.SelectionOptions.FindAsync(int.Parse(userAnswer.MyAnswers[0]));
                        var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == question.Id && a.Correct == true);
                        question.AnswerResult = new AnswerResultModel() { Correct = userAnswer.Correct, UserAnswer = userAnswerDetail.Content, CorrectAnswer = correctAnswer.Content, Analysis = questionTemp.Analysis };
                    }
                }
            }

            return question;
        }

        [HttpGet("{qid}/{aid}")]
        public async Task<QuestionModel> GetQuestionAsync(int qid, int aid)
        {
            var questionTemp = await _dbContext.Questions.FindAsync(qid);
            var question = new QuestionModel
            {
                Id = questionTemp.Id,
                Content = questionTemp.Content,
                Type = questionTemp.Type,
                Status = QuestionStatus.Untouched,
            };

            if (question.Type == QuestionType.SingleSelection)
            {
                var options = await _dbContext.SelectionOptions.Where(a => a.QuestionId == qid)
                    .Select(a => new QuestionModel.Option { Id = a.Id, Content = a.Content })
                    .ToListAsync();
                question.Options = options;
            }

            var userAnswer = await _dbContext.UserAnswers.FindAsync(aid);
            if (userAnswer != null)
            {
                question.Status = userAnswer.Correct ? QuestionStatus.CorrectAnswer : QuestionStatus.WrongAnswer;
                if (question.Type == QuestionType.SingleSelection)
                {
                    var userAnswerDetail = await _dbContext.SelectionOptions.FindAsync(int.Parse(userAnswer.MyAnswers[0]));
                    var correctAnswer = await _dbContext.SelectionOptions.FirstOrDefaultAsync(a => a.QuestionId == question.Id && a.Correct == true);
                    question.AnswerResult = new AnswerResultModel() { Correct = userAnswer.Correct, UserAnswer = userAnswerDetail.Content, CorrectAnswer = correctAnswer.Content, Analysis = questionTemp.Analysis };
                }
            }

            return question;
        }

        [HttpGet("MyEasyToGetWrongQuestions/{chapterId}")]
        public async Task GetMyEasyToGetWrongQuestionsAsync(int chapterId)
        {
            //獲取全部題目
            var questions = await _dbContext.Questions.Where(q => q.ChapterId == chapterId).ToListAsync();
            //獲取每個題目的錯誤次數
            foreach (var question in questions)
            {
                await _dbContext.UserAnswers.Where(a => a.QuestionId == question.Id && a.Correct == false).ToListAsync();
            }
            //返回
        }

        [HttpGet("EasyToGetWrongQuestionsByClass/{classId}")]
        public async Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestionsByClassAsync(int classId)
        {
            var studentIds = _dbContext.StudentsToClasses.Where(sc => sc.ClassRoomId == classId).Select(sc => sc.StudentId);
            var answers = _dbContext.UserAnswers.Where(a => studentIds.Contains(a.UserId) && a.Correct == false).AsEnumerable().GroupBy(a => a.QuestionId);
            var result = new CommonListResultModel<EasyToGetWrongQuestionModel> { Items = new List<EasyToGetWrongQuestionModel>() };
            foreach (var item in answers)
            {
                var question = await _dbContext.Questions.FindAsync(item.Key);
                result.Items.Add(new EasyToGetWrongQuestionModel { QuestionId = item.Key, Content = question.Content, ErrorCount = item.Count() });
            }

            return result;
        }
    }
}