using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Pages.Admin.Classrooms
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DetailModel : PageModel
    {
        private ApplicationDbContext _context;

        public DetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public int? ClassroomId { get; set; }
        [BindProperty]
        public int NewStudentId { get; set; }
        public PaginatedList<StudentToClass> Students { get; set; }

        public async Task<IActionResult> OnGetAsync(int? classroomId, string searchString, int pageIndex = 1)
        {
            if (classroomId == null)
            {
                return NotFound();
            }

            ClassroomId = classroomId;

            CurrentFilter = searchString?.Trim();
            var stocIQ = _context.StudentsToClasses.Include(s => s.Student).Where(s => s.ClassroomId == classroomId);
            if (CurrentFilter != null)
            {
                stocIQ = stocIQ.Where(c => c.Student.Nickname.ToLower().Contains(CurrentFilter.ToLower()));
            }

            Students = await PaginatedList<StudentToClass>.CreateAsync(stocIQ.AsNoTracking(), pageIndex, 10);

            return Page();
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var stoc = await _context.StudentsToClasses.FindAsync(id);
            if (stoc == null)
            {
                return Content("Error");
            }

            _context.StudentsToClasses.Remove(stoc);
            await _context.SaveChangesAsync();

            return Content("Ok");
        }

        public async Task<ContentResult> OnPostAddStudentAsync(int classroomId, int userId)
        {
            if (await _context.StudentsToClasses.AnyAsync(sc => sc.ClassroomId == classroomId && sc.StudentId == userId))
            {
                return Content("用户已在班级中");
            }

            if (!await _context.Users.AnyAsync(u => u.Id == userId && u.Role == UserRole.Student))
            {
                return Content("不存在该学生");
            }

            await _context.StudentsToClasses.AddAsync(new StudentToClass { ClassroomId = classroomId, StudentId = userId });
            await _context.SaveChangesAsync();

            return Content("Ok");
        }
    }
}