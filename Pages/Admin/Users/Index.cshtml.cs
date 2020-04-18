using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<User> Users { get; set; }

        public async Task OnGetAsync(string searchString, int pageIndex = 1)
        {
            CurrentFilter = searchString?.Trim();
            var userIQ = _context.Users.Select(u => u);
            if (CurrentFilter != null)
            {
                userIQ = _context.Users.Where(u => u.Nickname.ToLower().Contains(CurrentFilter.ToLower()));
            }

            Users = await PaginatedList<User>.CreateAsync(userIQ.AsNoTracking(), pageIndex, 10);
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Content("Error");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return Content("Ok");
        }
    }
}