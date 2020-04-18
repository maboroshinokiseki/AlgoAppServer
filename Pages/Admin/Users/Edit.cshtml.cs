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
    public class EditModel : PageModel
    {
        private ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserModel = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (UserModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("UserModel.Password");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserModel.Nickname == null)
            {
                UserModel.Nickname = UserModel.Username;
            }

            var user = await _context.Users.FindAsync(UserModel.Id);

            if (UserModel.Password == null)
            {
                UserModel.Password = user.Password;
            }
            else
            {
                UserModel.Password = Utilities.HashPassword(UserModel.Password);
            }

            ObjectMapper.Map(UserModel, user);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}