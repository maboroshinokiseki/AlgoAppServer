﻿namespace AlgoApp.Areas.Api.Models
{
    public class AnswerResultModel : CommonResultModel
    {
        public bool Correct { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public string Analysis { get; set; }
    }
}
