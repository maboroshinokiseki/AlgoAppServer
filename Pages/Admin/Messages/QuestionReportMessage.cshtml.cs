using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AlgoApp.Pages.Admin.Messages
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class QuestionReportMessageModel : PageModel
    {
        private ApplicationDbContext _context;

        public QuestionReportMessageModel(ApplicationDbContext context)
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
            var question = await _context.Questions.FindAsync(MessageContentDetail.QuestionId);
            MessageContentDetail.QuestionContent = question.Content;
            MessageContentDetail.ChapterId = question.ChapterId;

            return Page();
        }

        public class MessageContent
        {
            [Display(Name = "描述")]
            public string Content { get; set; }
            public int ChapterId { get; set; }
            public int QuestionId { get; set; }
            [Display(Name = "题目")]
            public string QuestionContent { get; set; }
        }
    }
}