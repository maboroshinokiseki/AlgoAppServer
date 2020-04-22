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
    public class CreateModel : PageModel
    {
        private ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int chapterid)
        {
            QuestionModel = new Question { ChapterId = chapterid };

            return Page();
        }

        [BindProperty]
        public Question QuestionModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var selectionOptions = QuestionModel.SelectionAnswers;
            if (!selectionOptions?.Any() ?? true)
            {
                ModelState.AddModelError(nameof(QuestionModel) + "." + nameof(QuestionModel.SelectionAnswers), "至少要有一个选项");
                return Page();
            }

            if (!selectionOptions.Any(o => o.Correct))
            {
                ModelState.AddModelError(nameof(QuestionModel) + "." + nameof(QuestionModel.SelectionAnswers), "至少要有一个正确答案");
                return Page();
            }

            if (QuestionModel.Analysis == null)
            {
                QuestionModel.Analysis = "无";
            }

            await _context.AddAsync(QuestionModel);
            await _context.SaveChangesAsync();

            return Redirect(".");
        }
    }
}