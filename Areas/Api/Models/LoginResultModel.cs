namespace AlgoApp.Areas.Api.Models
{
    public class LoginResultModel : CommonResultModel
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
