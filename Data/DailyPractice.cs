using System;

namespace AlgoApp.Data
{
    public class DailyPractice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }
}
