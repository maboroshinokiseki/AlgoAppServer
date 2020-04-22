using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public class Classroom
    {
        public int Id { get; set; }
        [Display(Name = "教师Id")]
        public int TeacherId { get; set; }
        public User Teacher { get; set; }
        [Display(Name = "班级名称")]
        [Required(ErrorMessage = "班级名称是必须的")]
        public string ClassName { get; set; }
        public List<StudentToClass> Students { get; set; }
    }
}
