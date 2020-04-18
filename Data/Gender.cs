using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public enum Gender
    {
        [Display(Name = "保密")]
        Secrecy,
        [Display(Name = "男")]
        Male,
        [Display(Name = "女")]
        Female
    }
}