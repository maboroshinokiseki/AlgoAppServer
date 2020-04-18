namespace AlgoApp.Data
{
    public class Bookmark
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; }
        public int QuestionId { get; set; }
        public Question Question { get; }
    }
}
