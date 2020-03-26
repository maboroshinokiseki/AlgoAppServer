using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Analysis { get; set; }
        public QuestionType Type { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public List<FillingAnswer> FillingAnswers { get; }
        public List<SelectionOption> SelectionAnswers { get; }
        public List<ResultTest> ResultTests { get; }
        public int Difficulty { get; set; }
    }
}
