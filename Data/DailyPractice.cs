﻿using System;

namespace AlgoApp.Data
{
    public class DailyPractice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }
}
