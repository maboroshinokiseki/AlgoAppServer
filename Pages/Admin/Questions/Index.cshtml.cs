using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Pages.Admin.Questions
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<Question> Questions { get; set; }

        public async Task OnGetAsync(int chapterid, string searchString, int pageIndex = 1)
        {
            CurrentFilter = searchString?.Trim();
            var questionIQ = _context.Questions.Where(q => q.ChapterId == chapterid);
            if (CurrentFilter != null)
            {
                questionIQ = questionIQ.Where(q => q.Content.ToLower().Contains(CurrentFilter.ToLower()));
            }

            Questions = await PaginatedList<Question>.CreateAsync(questionIQ.AsNoTracking(), pageIndex, 10);
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return Content("Error");
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return Content("Ok");
        }
    }
}