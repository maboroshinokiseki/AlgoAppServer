using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public class Question
    {
        public int Id { get; set; }
        [Display(Name = "题目")]
        [Required(ErrorMessage = "题目内容是必须的")]
        public string Content { get; set; }
        [Display(Name = "解析")]
        public string Analysis { get; set; }
        [Display(Name = "题目类型")]
        public QuestionType Type { get; set; }
        [Display(Name = "所属章节Id")]
        public int ChapterId { get; set; }
        public Chapter Chapter { get; }
        public List<FillingAnswer> FillingAnswers { get; }
        [Display(Name = "备选答案")]
        public List<SelectionOption> SelectionAnswers { get; }
        public List<ResultTest> ResultTests { get; }
        [Display(Name = "难度")]
        public int Difficulty { get; set; }
    }
}
