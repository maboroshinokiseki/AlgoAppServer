namespace AlgoApp.Data
{
    public class SelectionOption
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; }
        public bool Correct { get; set; }
    }
}
