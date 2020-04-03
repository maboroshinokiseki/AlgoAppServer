namespace AlgoApp.Areas.Api.Models
{
    public class LoginResultModel : CommonResultModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
