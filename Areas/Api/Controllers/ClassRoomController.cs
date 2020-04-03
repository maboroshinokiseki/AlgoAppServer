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
    public class ClassRoomController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ClassRoomController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<CommonListResultModel<ClassRoomModel>> MyClassRooms()
        {
            var uid = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == UserRole.Student.ToString())
            {
                return new CommonListResultModel<ClassRoomModel>
                {
                    Items = await _dbContext.StudentsToClasses.Where(sc => sc.StudentId == uid)
                                                                   .Select(sc => new ClassRoomModel { Id = sc.Id, Name = sc.ClassRoom.ClassName })
                                                                   .ToListAsync()
                };
            }
            else
            {
                return new CommonListResultModel<ClassRoomModel>
                {
                    Items = await _dbContext.ClassRooms.Where(c => c.TeacherId == uid)
                                                            .Select(c => new ClassRoomModel { Id = c.Id, Name = c.ClassName, StudentCount = c.Students.Count })
                                                            .ToListAsync()
                };
            }
        }

        public class NewClassModel
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public async Task<ClassRoomModel> AddClassRoom([FromBody] NewClassModel model)
        {
            var uid = int.Parse(User.Claims.GetClaim(ClaimTypes.NameIdentifier));
            var classRoom = new ClassRoom { ClassName = model.Name, TeacherId = uid };
            _dbContext.ClassRooms.Add(classRoom);
            await _dbContext.SaveChangesAsync();

            return new ClassRoomModel { Code = Codes.None, Id = classRoom.Id, Name = classRoom.ClassName };
        }

        [HttpDelete("{id}")]
        public async Task<CommonResultModel> DeleteClassRoom(int id)
        {
            var c = await _dbContext.ClassRooms.FindAsync(id);
            var scid = await _dbContext.StudentsToClasses.Where(sc => sc.ClassRoomId == id).Select(sc => new StudentToClass { Id = sc.Id }).ToListAsync();
            _dbContext.StudentsToClasses.RemoveRange(scid);
            _dbContext.ClassRooms.Remove(c);
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        [HttpPost]
        public async Task<CommonListResultModel<ClassRoomModel>> SearchClassRomm(string searchText)
        {
            if (int.TryParse(searchText, out var id))
            {
                return new CommonListResultModel<ClassRoomModel>
                {
                    Items = await _dbContext.ClassRooms.Where(c => c.Id == id || c.ClassName == searchText || c.Teacher.NickName == searchText)
                                                            .Select(c => new ClassRoomModel { Id = c.Id, Name = c.ClassName, Teacher = new UserModel { NickName = c.Teacher.NickName } })
                                                            .ToListAsync()
                };
            }
            else
            {
                return new CommonListResultModel<ClassRoomModel>
                {
                    Items = await _dbContext.ClassRooms.Where(c => c.ClassName == searchText || c.Teacher.NickName == searchText)
                                                            .Select(c => new ClassRoomModel { Id = c.Id, Name = c.ClassName, Teacher = new UserModel { NickName = c.Teacher.NickName } })
                                                            .ToListAsync()
                };
            }
        }

        public async Task<CommonResultModel> JoinClassRomm(int id)
        {
            var uid = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _dbContext.StudentsToClasses.Add(new StudentToClass { ClassRoomId = id, StudentId = uid });
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        public async Task<CommonResultModel> RemoveFromClassRomm(int cid, int uid)
        {
            var stoc = await _dbContext.StudentsToClasses.FirstAsync(sc => sc.ClassRoomId == cid && sc.StudentId == uid);
            _dbContext.Remove(stoc);
            await _dbContext.SaveChangesAsync();

            return new CommonResultModel { Code = Codes.None };
        }

        [HttpGet("{id}")]
        public async Task<ClassRoomModel> ClassRoom(int id)
        {
            var classRoom = await _dbContext.ClassRooms.FindAsync(id);
            await _dbContext.Entry(classRoom).Collection(c => c.Students).LoadAsync();
            var students = new List<UserModel>();
            var userControll = new UserController(_dbContext, null);
            foreach (var sc in classRoom.Students)
            {
                students.Add(await userControll.UserDetail(sc.StudentId));
            }

            return new ClassRoomModel { Id = classRoom.Id, Name = classRoom.ClassName, Students = students, StudentCount = students.Count };
        }

        public class RenameClassRommPostModel
        {
            public string NewName { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<CommonResultModel> RenameClassRomm(int id, [FromBody] RenameClassRommPostModel model)
        {
            var classRoom = await _dbContext.ClassRooms.FindAsync(id);
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
            if (await _dbContext.StudentsToClasses.Where(sc => sc.StudentId == model.StudentId && sc.ClassRoomId == model.ClassId).CountAsync() != 0)
            {
                return new CommonResultModel { Code = Codes.UserAlreadyInClass };
            }

            await _dbContext.StudentsToClasses.AddAsync(new StudentToClass { ClassRoomId = model.ClassId, StudentId = model.StudentId });
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }

        [HttpPost]
        public async Task<CommonResultModel> RemoveStudentFromClass([FromBody] AddStudentToClassPostModel model)
        {
            var stoc = await _dbContext.StudentsToClasses.FirstAsync(sc => sc.StudentId == model.StudentId && sc.ClassRoomId == model.ClassId);
            _dbContext.StudentsToClasses.Remove(stoc);
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }
    }
}