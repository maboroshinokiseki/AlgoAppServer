using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Models
{
    public class HistoryItemModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public bool Correct { get; set; }
    }
}
