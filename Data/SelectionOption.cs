using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public class SelectionOption
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public bool Correct { get; set; }
    }
}
