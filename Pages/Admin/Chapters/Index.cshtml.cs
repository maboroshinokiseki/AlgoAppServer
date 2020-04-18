using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Pages.Admin.Chapters
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Chapter> Chapters { get; set; }

        public async Task OnGetAsync()
        {
            Chapters = await _context.Chapters.OrderBy(c => c.Order).AsNoTracking().ToListAsync();
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var chapter= await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return Content("Error");
            }

            var chapters = await _context.Chapters.Where(c => c.Order > chapter.Order).ToListAsync();
            foreach (var item in chapters)
            {
                --item.Order;
            }
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return Content("Ok");
        }
    }
}