﻿using AlgoApp.Data;
using System.Collections.Generic;

namespace AlgoApp.Areas.Api.Models
{
    public class QuestionModel : CommonResultModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public List<Option> Options { get; set; }
        public QuestionStatus Status { get; set; }
        public AnswerResultModel AnswerResult { get; set; }

        public class Option
        {
            public int Id { get; set; }
            public string Content { get; set; }
        }
    }
}
