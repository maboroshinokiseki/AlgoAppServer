using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public enum UserRole
    {
        [Display(Name = "管理员")]
        Admin,
        [Display(Name = "教师")]
        Teacher,
        [Display(Name = "学生")]
        Student,
    }
}
