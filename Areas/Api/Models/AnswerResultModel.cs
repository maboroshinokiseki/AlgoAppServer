using System.Collections.Generic;

namespace AlgoApp.Areas.Api.Models
{
    public class AnswerResultModel : CommonResultModel
    {
        public bool Correct { get; set; }
        public List<string> UserAnswers { get; set; }
        public List<string> CorrectAnswers { get; set; }
    }
}
