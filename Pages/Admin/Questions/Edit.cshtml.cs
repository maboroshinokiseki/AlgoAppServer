using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Pages.Admin.Questions
{
    public class EditModel : PageModel
    {
        private ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question QuestionModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuestionModel = await _context.Questions.Include(q => q.SelectionAnswers).FirstOrDefaultAsync(q => q.Id == id);

            if (QuestionModel == null)
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

            var question = await _context.Questions.Include(q => q.SelectionAnswers).Where(q => q.Id == QuestionModel.Id).FirstOrDefaultAsync();
            var newSelectionOptions = selectionOptions.Where(o => question.SelectionAnswers.All(o2 => o2.Id != o.Id));
            var deletedSelectionOptions = question.SelectionAnswers.Where(o => selectionOptions.All(o2 => o2.Id != o.Id));
            var modifiedSelectionOptions = selectionOptions.Intersect(question.SelectionAnswers, new IdComparer());

            foreach (var item in modifiedSelectionOptions)
            {
                var temp = question.SelectionAnswers.First(o => o.Id == item.Id);
                temp.Content = item.Content;
                temp.Correct = item.Correct;
            }

            await _context.SelectionOptions.AddRangeAsync(newSelectionOptions);
            _context.SelectionOptions.RemoveRange(deletedSelectionOptions);

            ObjectMapper.Map(QuestionModel, question);

            await _context.SaveChangesAsync();

            return Redirect(".");
        }

        class IdComparer : IEqualityComparer<SelectionOption>
        {
            public bool Equals([AllowNull] SelectionOption x, [AllowNull] SelectionOption y) => x?.Id == y?.Id;

            public int GetHashCode([DisallowNull] SelectionOption obj) => obj.Id.GetHashCode();

        }
    }
}