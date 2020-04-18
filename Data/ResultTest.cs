namespace AlgoApp.Data
{
    public class ResultTest
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; }
    }
}
