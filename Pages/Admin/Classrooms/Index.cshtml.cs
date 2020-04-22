using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Pages.Admin.Classrooms
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<ClassroomModel> Classrooms { get; set; }

        public async Task OnGetAsync(string searchString, int pageIndex = 1)
        {
            CurrentFilter = searchString?.Trim();
            var classIQ = _context.Classrooms.Select(c => new ClassroomModel { Id = c.Id, Teacher = new UserModel { Nickname = c.Teacher.Nickname }, Name = c.ClassName, StudentCount = c.Students.Count() });
            if (CurrentFilter != null)
            {
                classIQ = classIQ.Where(c => c.Name.ToLower().Contains(CurrentFilter.ToLower()));
            }

            Classrooms = await PaginatedList<ClassroomModel>.CreateAsync(classIQ.AsNoTracking(), pageIndex, 10);
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return Content("Error");
            }

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();

            return Content("Ok");
        }

        public async Task<ContentResult> OnPostRenameAsync(int? id, string newName)
        {
            if (id == null || string.IsNullOrWhiteSpace(newName))
            {
                return Content("Error");
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return Content("Error");
            }

            classroom.ClassName = newName.Trim();
            await _context.SaveChangesAsync();

            return Content("Ok");
        }
    }
}