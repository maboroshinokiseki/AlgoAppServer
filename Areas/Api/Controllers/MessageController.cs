using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoApp.Areas.Api.Models;
using AlgoApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlgoApp.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public MessageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<CommonResultModel> PostMessage([FromBody] Message message)
        {
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return new CommonResultModel { Code = Codes.None };
        }
    }
}