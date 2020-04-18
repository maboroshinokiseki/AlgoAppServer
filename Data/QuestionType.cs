using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public enum QuestionType
    {
        [Display(Name = "单选题")]
        SingleSelection,
        [Display(Name = "复选题")]
        MultiSelection,
        //CodeFilling,
        //TextFilling,
    }
}
