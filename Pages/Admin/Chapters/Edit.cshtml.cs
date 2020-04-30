using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Pages.Admin.Chapters
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EditModel : PageModel
    {
        private ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Chapter Chapter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == id);

            if (Chapter == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Chapter.Name = Chapter.Name.Trim();
            var chapter = await _context.Chapters.FindAsync(Chapter.Id);

            if (Chapter.Order > await _context.Chapters.CountAsync())
            {
                ModelState.AddModelError("Chapter.Order", "顺序不能大于最大顺序");
                return Page();
            }

            if (await _context.Chapters.AnyAsync(c => c.Name.ToLower() == Chapter.Name.ToLower()))
            {
                ModelState.AddModelError("Chapter.Name", "已存在同名章节");
                return Page();
            }

            if (Chapter.Order < chapter.Order)
            {
                var chapters = await _context.Chapters.Where(c => Chapter.Order <= c.Order && c.Order <= chapter.Order).ToListAsync();
                foreach (var item in chapters)
                {
                    ++item.Order;
                }
            }
            else if (chapter.Order < Chapter.Order)
            {
                var chapters = await _context.Chapters.Where(c => chapter.Order <= c.Order && c.Order <= Chapter.Order).ToListAsync();
                foreach (var item in chapters)
                {
                    --item.Order;
                }
            }

            ObjectMapper.Map(Chapter, chapter);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}