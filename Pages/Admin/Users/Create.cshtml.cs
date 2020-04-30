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

namespace AlgoApp.Pages.Admin.Users
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
        public User UserModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _context.Users.Where(u => u.Username == UserModel.Username).FirstOrDefaultAsync() != null)
            {
                ModelState.AddModelError("UserModel.Username", "用户已存在");
                return Page();
            }

            if (UserModel.Nickname == null)
            {
                UserModel.Nickname = UserModel.Username;
            }

            UserModel.Password = Utilities.HashPassword(UserModel.Password);
            await _context.Users.AddAsync(UserModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}