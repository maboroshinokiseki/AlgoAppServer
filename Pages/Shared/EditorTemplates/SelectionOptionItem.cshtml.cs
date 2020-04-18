using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlgoApp.Data;

namespace AlgoApp.Pages.Shared.EditorTemplates
{
    public class SelectionOptionItemModel : PageModel
    {
        private readonly AlgoApp.Data.ApplicationDbContext _context;

        public SelectionOptionItemModel(AlgoApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SelectionOption SelectionOption { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SelectionOption = await _context.SelectionOptions
                .Include(s => s.Question).FirstOrDefaultAsync(m => m.Id == id);

            if (SelectionOption == null)
            {
                return NotFound();
            }
           ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Content");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SelectionOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SelectionOptionExists(SelectionOption.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SelectionOptionExists(int id)
        {
            return _context.SelectionOptions.Any(e => e.Id == id);
        }
    }
}
