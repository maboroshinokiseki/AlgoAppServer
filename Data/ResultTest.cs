using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Data
{
    public class ResultTest
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
