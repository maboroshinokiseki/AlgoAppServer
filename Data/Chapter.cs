using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public class Chapter
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "章节名称是必须的")]
        [Display(Name = "章节名称")]
        public string Name { get; set; }
        [Display(Name = "顺序")]
        public int Order { get; set; }
        public List<Question> Questions { get; }
    }
}
