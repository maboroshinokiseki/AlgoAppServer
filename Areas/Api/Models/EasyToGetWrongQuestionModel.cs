using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Models
{
    public class EasyToGetWrongQuestionModel
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int ErrorCount { get; set; }
    }
}
