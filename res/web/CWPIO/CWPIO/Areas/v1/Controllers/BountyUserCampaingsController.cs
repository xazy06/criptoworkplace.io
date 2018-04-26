using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWPIO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CWPIO.Areas.v1.Controllers
{
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/bounty/usercampaing")]
    public class BountyUserCampaingsController : Controller
    {
        private ApplicationDbContext _dbContext;
        public BountyUserCampaingsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
                return NotFound();
            
            var result = _dbContext
                .UserBounties
                .Include(b => b.Items)
                .Where(t => t.UserId == user.Id);

            if (await result.AnyAsync())
                return Ok(await result.ToListAsync());

            return NotFound();
        }

        [HttpGet("{bountyId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string bountyId)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _dbContext.FindAsync<UserBountyCampaing>(user.Id, bountyId);
            await _dbContext.Entry(result).Collection(b => b.Items).LoadAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}