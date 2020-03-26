using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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
                var user = await GetUser(model.Username, model.Password);

                if (user == null)
                {
                    return new LoginResultModel { Code = Codes.LoginFailed, Description = "Wrong Username or Password." };
                }

                return new LoginResultModel { Code = Codes.None, Description = "Success.", Role = user.Role.ToString(), Token = GenarateToken(user) };
            }

            return new LoginResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [HttpPost]
        public async Task<LoginResultModel> Register([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetUser(model.Username);

                if (user != null)
                {
                    return new LoginResultModel { Code = Codes.RegistrationFailed, Description = "User already exists." };
                }

                user = new User { Username = model.Username, Password = model.Password, NickName = model.Username, Role = UserRole.Student };

                await AddUser(user);

                return new LoginResultModel { Code = Codes.None, Description = "Success.", Token = GenarateToken(user) };
            }

            return new LoginResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UserModel> CurrentUser()
        {
            var user = await GetUser(User.Identity.Name);
            return new UserModel { Code = Codes.None, Username = user.Username, NickName = user.NickName, Role = user.Role };
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

        private string GenarateToken(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}