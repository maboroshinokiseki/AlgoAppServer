using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AlgoApp.Pages.Admin.Classrooms
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CreateModel : PageModel
    {
        private ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Classroom Classroom { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Classroom.ClassName = Classroom.ClassName.Trim();

            if (await _context.Classrooms.AnyAsync(c => c.ClassName.ToLower() == Classroom.ClassName.ToLower()))
            {
                ModelState.AddModelError($"{nameof(Classroom)}.{nameof(Classroom.ClassName)}", "已存在同名班级");
                return Page();
            }

            if (!await _context.Users.AnyAsync(u => u.Id == Classroom.TeacherId && u.Role == UserRole.Teacher))
            {
                ModelState.AddModelError($"{nameof(Classroom)}.{nameof(Classroom.TeacherId)}", "不存在对应的教师");
                return Page();
            }

            await _context.Classrooms.AddAsync(Classroom);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}