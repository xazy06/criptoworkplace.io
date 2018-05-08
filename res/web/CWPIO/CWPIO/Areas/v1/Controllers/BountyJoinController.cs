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
            await _dbContext.Entry(user).Collection(u => u.BountyUserCampaings).LoadAsync();

            if (user.BountyUserCampaings.Any(b => b.BountyCampaingId == bountyId))
            {
                return BadRequest("Already joined to this bounty program");
            }

            var item = new BountyUserCampaing { BountyCampaing = bountyCampaing, CreatedByUser = user };
            user.BountyUserCampaings.Add(item);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAsync", "BountyUserCampaingsController", new { bountyId }, item);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromRoute]string bountyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.GetCurrentUserAsync(User, "BountyUserCampaings", "BountyCampaingAcceptedTasks.BountyCampaingTask");
            if (user == null)
            {
                return NotFound();
            }
            //await _dbContext.Entry(user).Collection(u => u.BountyUserCampaings).LoadAsync();

            var item = user.BountyUserCampaings.SingleOrDefault(b => b.BountyCampaingId == bountyId);
            if (item == null)
            {
                return NotFound();
            }
            //await _dbContext.Entry(user).Collection(u => u.BountyCampaingAcceptedTasks).LoadAsync();
            if (user.BountyCampaingAcceptedTasks.Any(i =>
            {
              //  _dbContext.Entry(i).Reference(t => t.BountyCampaingTask).Load();
                return i.Status == BountyCampaingTaskStatus.Moderation && i.BountyCampaingTask.BountyCampaingId == bountyId;
            }))
            {
                return BadRequest("Can not unsubscribe, when task on moderations");
            }

            item.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return Ok(item);
        }


    }
}