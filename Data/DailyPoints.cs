﻿using System;

namespace AlgoApp.Data
{
    public class DailyPoints
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
    }
}
