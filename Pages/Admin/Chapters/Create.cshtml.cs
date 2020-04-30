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

namespace AlgoApp.Pages.Admin.Chapters
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
        public Chapter Chapter { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Chapter.Name = Chapter.Name.Trim();

            if (Chapter.Order > await _context.Chapters.CountAsync()+1)
            {
                ModelState.AddModelError("Chapter.Order", "顺序不能大于最大顺序");
                return Page();
            }

            if (await _context.Chapters.AnyAsync(c => c.Name.ToLower() == Chapter.Name.ToLower()))
            {
                ModelState.AddModelError("Chapter.Name", "已存在同名章节");
                return Page();
            }

            var chapters = await _context.Chapters.Where(c => Chapter.Order <= c.Order).ToListAsync();
            foreach (var item in chapters)
            {
                ++item.Order;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}