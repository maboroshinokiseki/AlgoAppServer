using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Data
{
    public class Message
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
}
