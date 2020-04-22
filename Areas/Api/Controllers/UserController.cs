using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
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
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<LoginResultModel> Login([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetUser(model.Username, Utilities.HashPassword(model.Password));

                if (user == null)
                {
                    return new LoginResultModel { Code = Codes.LoginFailed, Description = "Wrong Username or Password." };
                }

                return new LoginResultModel { Code = Codes.None, Description = "Success.", UserId = user.Id, Role = user.Role.ToString(), Token = GenarateToken(user) };
            }

            return new LoginResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [AllowAnonymous]
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

                user = new User { Username = model.Username.ToLower(), Password = Utilities.HashPassword(model.Password), Nickname = model.Username, Role = UserRole.Student };

                await AddUser(user);

                return new LoginResultModel { Code = Codes.None, Description = "Success.", UserId = user.Id, Token = GenarateToken(user) };
            }

            return new LoginResultModel { Code = Codes.Unknown, Description = "Unknow error." };
        }

        [HttpGet]
        public async Task<UserModel> CurrentUser()
        {
            var uid = int.Parse(User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            return await UserDetail(uid);
        }

        [HttpGet("{id}")]
        public async Task<UserModel> UserDetail(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            double answerCount = await _dbContext.UserAnswers.Where(a => a.UserId == id).CountAsync();
            double correctCount = await _dbContext.UserAnswers.Where(a => a.UserId == id && a.Correct == true).CountAsync();
            var ratio = answerCount == 0 ? 0 : correctCount / answerCount;
            var doneCount = await _dbContext.UserAnswers.Where(a => a.UserId == id).CountAsync();
            var result = new UserModel { Code = Codes.None, CorrectRatio = ratio, DoneQuestionCount = doneCount };
            user.Password = "";
            return ObjectMapper.Map(user, result);
        }

        public async Task<CommonResultModel> UpdateUserInfo([FromBody] UserModel model)
        {
            var user = await _dbContext.Users.FindAsync(model.Id);
            user.BirthDay = model.BirthDay;
            user.Gender = model.Gender;
            user.Nickname = model.Nickname;
            if (model.Password != "")
            {
                user.Password = Utilities.HashPassword(model.Password);
            }
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }

        [HttpGet("{id}/{name}")]
        public async Task<CommonListResultModel<UserModel>> SearchStudentsNotInClass(int id, string name)
        {
            var students = await _dbContext.Users.Where(u => u.Role == UserRole.Student &&
                                                        u.Nickname.Contains(name) &&
                                                        _dbContext.StudentsToClasses.Where(c => c.ClassroomId == id && c.StudentId == u.Id).Count() == 0)
                                                 .ToListAsync();
            var result = new CommonListResultModel<UserModel> { Items = new List<UserModel>() };
            foreach (var s in students)
            {
                result.Items.Add(new UserModel { Id = s.Id, Nickname = s.Nickname });
            }

            return result;
        }

        public async Task<CommonListResultModel<UserModel>> YesterdayTop10()
        {
            var items = await _dbContext.DailyPoints.Where(d => d.Date == DateTime.Today.AddDays(-1))
                                                    .OrderByDescending(d => d.Points)
                                                    .Take(10)
                                                    .Select(d => new UserModel { Nickname = d.User.Nickname, Points = d.Points })
                                                    .ToListAsync();
            return new CommonListResultModel<UserModel> { Code = Codes.None, Items = items };
        }

        public async Task<CommonListResultModel<UserModel>> AllTimeTop10()
        {
            var items = await _dbContext.Users.Where(u => u.Points != 0)
                                              .OrderByDescending(u => u.Points)
                                              .Take(10)
                                              .Select(u => new UserModel { Nickname = u.Nickname, Points = u.Points })
                                              .ToListAsync();

            return new CommonListResultModel<UserModel> { Code = Codes.None, Items = items };
        }

        private async Task<User> GetUser(string username, string password = null)
        {
            if (password == null)
            {
                return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username.ToLower());
            }
            else
            {
                return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username.ToLower() && u.Password == password && u.Role != UserRole.Admin);
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