using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public enum MessageType
    {
        [Display(Name = "题目推荐")]
        NewQuestion,
        [Display(Name = "题目纠错")]
        QuestionReport,
    }
}
