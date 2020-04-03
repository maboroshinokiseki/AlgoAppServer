using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public ChaptersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<CommonListResultModel<Chapter>> GetChaptersAsync()
        {
            return new CommonListResultModel<Chapter> { Code = Codes.None, Items = await _dbContext.Chapters.OrderBy(c => c.Order).ToListAsync() };
        }

        [HttpGet("{cid}/questions")]
        public async Task<CommonListResultModel<Question>> GetQuestionsAsync(int cid)
        {
            return new CommonListResultModel<Question> { Code = Codes.None, Items = await _dbContext.Questions.Where(q => q.ChapterId == cid).ToListAsync() };
        }
    }
}