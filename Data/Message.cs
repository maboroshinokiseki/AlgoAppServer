namespace AlgoApp.Data
{
    public class Message
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        public int UserId { get; set; }
        public User User { get; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
}
