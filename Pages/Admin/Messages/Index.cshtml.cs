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

namespace AlgoApp.Pages.Admin.Messages
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Message> Messages { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            Messages = await PaginatedList<Message>.CreateAsync(_context.Messages.OrderByDescending(m => m.Id).AsNoTracking(), pageIndex, 10);
        }

        public async Task<ContentResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return Content("Error");
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return Content("Error");
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return Content("Ok");
        }
    }
}