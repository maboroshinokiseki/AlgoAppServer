using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlgoApp.Pages.Admin.Messages
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class NewQuestionMessageModel : PageModel
    {
        private ApplicationDbContext _context;

        public NewQuestionMessageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Message Message { get; set; }
        public MessageContent MessageContentDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Message = await _context.Messages.FindAsync(id);
            if (Message == null)
            {
                return NotFound();
            }
            Message.Read = true;
            await _context.SaveChangesAsync();

            MessageContentDetail = JsonSerializer.Deserialize<MessageContent>(Message.Content);
            var chapter = await _context.Chapters.FindAsync(MessageContentDetail.ChapterId);
            MessageContentDetail.ChapterName = $"第{chapter.Order}章 {chapter.Name}";
            return Page();
        }

        public class MessageContent
        {
            public int ChapterId { get; set; }
            [Display(Name = "章节")]
            public string ChapterName { get; set; }
            [Display(Name = "题目")]
            public string Content { get; set; }
            public List<Option> Options { get; set; }
            [Display(Name = "解析")]
            public string Analysis { get; set; }
            [Display(Name = "难度")]
            public int Difficulty { get; set; }
        }

        public class Option
        {
            public string OptionText { get; set; }
            public bool IsCorrect { get; set; }
        }
    }
}