using System;
using System.Collections.Generic;

namespace AlgoApp.Data
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public bool Correct { get; set; }
        public List<string> MyAnswers { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
