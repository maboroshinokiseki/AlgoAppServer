using AlgoApp.Data;

namespace AlgoApp.Areas.Api.Models
{
    public class UserModel : CommonResultModel
    {
        public string Username { get; set; }
        public string NickName { get; set; }
        public UserRole Role { get; set; }
        public double CorrectRatio { get; set; }
    }
}
