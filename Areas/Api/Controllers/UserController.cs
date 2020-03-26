using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public class PostModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<LoginResultModel> Login([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await Logout();
                }

                var user = await GetUser(model.Username, model.Password);

                if (user == null)
                {
                    return new LoginResultModel { Code = Codes.LoginFailed, Description = "Wrong Username or Password." };
                }

                await SignIn(user);

                return new LoginResultModel { Code = Codes.None, Description = "Success.", Role = user.Role.ToString() };
            }

            return new LoginResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [HttpPost]
        public async Task<CommonResultModel> Register([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await Logout();
                }

                var user = await GetUser(model.Username);

                if (user != null)
                {
                    return new CommonResultModel { Code = Codes.RegistrationFailed, Description = "User already exists." };
                }

                user = new User { Username = model.Username, Password = model.Password, NickName = model.Username, Role = UserRole.Student };

                await AddUser(user);

                await SignIn(user);

                return new CommonResultModel { Code = Codes.None, Description = "Success." };
            }

            return new CommonResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [HttpGet]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task< UserModel> CurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetUser(User.Identity.Name);
                return new UserModel { Code = Codes.None, Username = user.Username, NickName = user.NickName, Role = user.Role };
            }
            return new UserModel() { Code = Codes.LoginFailed };
        }

        private async Task<User> GetUser(string username, string password = null)
        {
            if (password == null)
            {
                return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            }
            else
            {
                return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password && u.Role != UserRole.Admin);
            }

        }

        private async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SignIn(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMonths(1),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}