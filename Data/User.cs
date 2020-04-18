using System;
using System.ComponentModel.DataAnnotations;

namespace AlgoApp.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "用户名是必须的")]
        [Display(Name = "用户名")]
        public string Username { get; set; }
        [Required(ErrorMessage = "密码是必须的")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "昵称")]
        public string Nickname { get; set; }
        [Display(Name = "用户类型")]
        public UserRole Role { get; set; }
        [Display(Name = "积分")]
        public int Points { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
        [Display(Name = "出生日期")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
    }
}
