using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using AlgoApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClassroomController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ClassroomController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<CommonListResultModel<ClassroomModel>> MyClassrooms()
        {
            var uid = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (User.Claims.GetClaim(ClaimTypes.Role) == UserRole.Student.ToString())
            {
                return new CommonListResultModel<ClassroomModel>
                {
                    Items = await _dbContext.StudentsToClasses.Where(sc => sc.StudentId == uid)
                                                              .Select(sc => new ClassroomModel { Id = sc.Id, Name = sc.Classroom.ClassName, Teacher = new UserModel { Nickname = sc.Classroom.Teacher.Nickname } })
                                                              .ToListAsync()
                };
            }
            else
            {
                return new CommonListResultModel<ClassroomModel>
                {
                    Items = await _dbContext.Classrooms.Where(c => c.TeacherId == uid)
                                                       .Select(c => new ClassroomModel { Id = c.Id, Name = c.ClassName, StudentCount = c.Students.Count })
                                                       .ToListAsync()
                };
            }
        }

        public class NewClassModel
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public async Task<ClassroomModel> AddClassroom([FromBody] NewClassModel model)
        {
            var uid = int.Parse(User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var classRoom = new Classroom { ClassName = model.Name, TeacherId = uid };
            _dbContext.Classrooms.Add(classRoom);
            await _dbContext.SaveChangesAsync();

            return new ClassroomModel { Code = Codes.None, Id = classRoom.Id, Name = classRoom.ClassName };
        }

        [HttpDelete("{id}")]
        public async Task<CommonResultModel> DeleteClassroom(int id)
        {
            var c = await _dbContext.Classrooms.FindAsync(id);
            var scid = await _dbContext.StudentsToClasses.Where(sc => sc.ClassroomId == id).Select(sc => new StudentToClass { Id = sc.Id }).ToListAsync();
            _dbContext.StudentsToClasses.RemoveRange(scid);
            _dbContext.Classrooms.Remove(c);
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        [HttpGet("{searchText}")]
        public async Task<CommonListResultModel<ClassroomModel>> SearchClassImNotIn(string searchText)
        {
            var uid = int.Parse(User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            return new CommonListResultModel<ClassroomModel>
            {
                Items = await _dbContext.Classrooms.Where(c => (c.ClassName.Contains(searchText) || c.Teacher.Nickname.Contains(searchText)) && !_dbContext.StudentsToClasses.Any(sc => sc.StudentId == uid && sc.ClassroomId == c.Id))
                                                   .Select(c => new ClassroomModel { Id = c.Id, Name = c.ClassName, Teacher = new UserModel { Nickname = c.Teacher.Nickname } })
                                                   .ToListAsync()
            };
        }

        [HttpGet("{id}")]
        public async Task<CommonResultModel> JoinClassRomm(int id)
        {
            var uid = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _dbContext.StudentsToClasses.Add(new StudentToClass { ClassroomId = id, StudentId = uid });
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        public async Task<CommonResultModel> RemoveFromClassRomm(int cid, int uid)
        {
            var stoc = await _dbContext.StudentsToClasses.FirstAsync(sc => sc.ClassroomId == cid && sc.StudentId == uid);
            _dbContext.Remove(stoc);
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        [HttpGet("{id}")]
        public async Task<ClassroomModel> Classroom(int id)
        {
            var classRoom = await _dbContext.Classrooms.FindAsync(id);
            await _dbContext.Entry(classRoom).Collection(c => c.Students).LoadAsync();
            var students = new List<UserModel>();
            var userControll = new UserController(_dbContext, null);
            foreach (var sc in classRoom.Students)
            {
                students.Add(await userControll.UserDetail(sc.StudentId));
            }

            return new ClassroomModel { Id = classRoom.Id, Name = classRoom.ClassName, Students = students, StudentCount = students.Count };
        }

        public class RenameClassRommPostModel
        {
            public string NewName { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<CommonResultModel> RenameClassRomm(int id, [FromBody] RenameClassRommPostModel model)
        {
            var classRoom = await _dbContext.Classrooms.FindAsync(id);
            classRoom.ClassName = model.NewName;
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        public class AddStudentToClassPostModel
        {
            public int StudentId { get; set; }
            public int ClassId { get; set; }
        }

        [HttpPost]
        public async Task<CommonResultModel> AddStudentToClass([FromBody] AddStudentToClassPostModel model)
        {
            if (await _dbContext.StudentsToClasses.Where(sc => sc.StudentId == model.StudentId && sc.ClassroomId == model.ClassId).CountAsync() != 0)
            {
                return new CommonResultModel { Code = Codes.RecordExists };
            }

            await _dbContext.StudentsToClasses.AddAsync(new StudentToClass { ClassroomId = model.ClassId, StudentId = model.StudentId });
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }

        [HttpPost]
        public async Task<CommonResultModel> RemoveStudentFromClass([FromBody] AddStudentToClassPostModel model)
        {
            var stoc = await _dbContext.StudentsToClasses.FirstAsync(sc => sc.StudentId == model.StudentId && sc.ClassroomId == model.ClassId);
            _dbContext.StudentsToClasses.Remove(stoc);
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }
    }
}