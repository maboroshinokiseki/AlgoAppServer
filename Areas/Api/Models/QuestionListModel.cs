using AlgoApp.Data;
using System.Collections.Generic;

namespace AlgoApp.Areas.Api.Models
{
    public class QuestionListModel : CommonResultModel
    {
        public List<Question> Questions { get; set; }
    }
}
