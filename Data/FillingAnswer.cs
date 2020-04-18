namespace AlgoApp.Data
{
    public class FillingAnswer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; }
        public int Order { get; set; }
    }
}
