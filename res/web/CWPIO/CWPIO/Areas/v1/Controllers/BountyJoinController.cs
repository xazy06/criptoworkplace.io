using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CWPIO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CWPIO.Areas.v1.Controllers
{
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/bounty/{bountyId}/join")]
    public class BountyJoinController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BountyJoinController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromRoute]string bountyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var bountyCampaing = await _dbContext.FindAsync<BountyCampaing>(bountyId);
            if (bountyCampaing == null)
            {
                return NotFound();
            }
            await _dbContext.Entry(bountyCampaing).Collection(b => b.UserBounties).LoadAsync();

            if (bountyCampaing.UserBounties.Any(b => b.UserId == user.Id))
            {
                return BadRequest("Already joined to this bounty program");
            }

            var item = new UserBountyCampaing { User = user };
            bountyCampaing.UserBounties.Add(item);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAsync" , "BountyUserCampaingsController",new { bountyId } , item);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromRoute]string bountyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var bountyCampaing = await _dbContext.FindAsync<BountyCampaing>(bountyId);
            if (bountyCampaing == null)
            {
                return NotFound();
            }
            await _dbContext.Entry(bountyCampaing).Collection(b => b.UserBounties).LoadAsync();

            var item = bountyCampaing.UserBounties.SingleOrDefault(b => b.UserId == user.Id);
            if (item == null)
            {
                return NotFound();
            }
            await _dbContext.Entry(item).Collection(b => b.Items).LoadAsync();
            if (item.Items.Any(i => i.IsAccepted.GetValueOrDefault()))
            {
                return BadRequest("Can not unsubscribe, when approved actions present");
            }
            else
            {
                _dbContext.RemoveRange(item.Items);
            }
            _dbContext.Remove(item);

            await _dbContext.SaveChangesAsync();
            
            return Ok(item);
        }

        
    }
}