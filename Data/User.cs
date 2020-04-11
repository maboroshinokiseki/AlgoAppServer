using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Nickname { get; set; }
        public UserRole Role { get; set; }
        public int Points { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
