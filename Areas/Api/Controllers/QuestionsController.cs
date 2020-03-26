using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var uid = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
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
    }
}