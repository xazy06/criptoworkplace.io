using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWPIO.Areas.v1.Models;
using CWPIO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CWPIO.Areas.v1.Controllers
{
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/bounty/usercampaing/{bountyId}/items")]
    public class BountyUserCampaingItemsController : Controller
    {
        private ApplicationDbContext _dbContext;
        public BountyUserCampaingItemsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute]string bountyId, [FromQuery]bool includeDeleted = false)
        {
            var result = _dbContext
                .BountyCampaingTasks
                .Where(t => t.BountyCampaingId == bountyId);

            if (!includeDeleted)
                result = result.Where(b => !b.IsDeleted);
            if (await result.AnyAsync())
                return Ok(await result.ToListAsync());

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]string bountyId, [FromRoute] string id)
        {
            var result = await _dbContext.FindAsync<BountyCampaingTask>(id);

            if (result == null || result.BountyCampaingId != bountyId)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromRoute]string bountyId, [FromBody]SimpleUserBountyItemDto bountyItem)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
                return NotFound();

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var bountyCampaing = await _dbContext.FindAsync<BountyUserCampaing>(bountyId);
            //if (bountyCampaing == null)
            //{
            //    return NotFound();
            //}

            //await _dbContext.Entry(bountyCampaing).Collection(b => b.Items).LoadAsync();

            //var newBountyItem = new BountyCampaingTask
            //{
            //    ItemType = bountyItem.ItemType.Value,
            //    Url = bountyItem.Url,
            //    UserId = user.Id
            //};
            //bountyCampaing.Items.Add(newBountyItem);

            //await _dbContext.SaveChangesAsync();

            //return CreatedAtAction("GetAsync", new { id = newBountyItem.Id }, newBountyItem);
            return Ok();
        }
        
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]string bountyId, [FromRoute]string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaingItem = await _dbContext.FindAsync<BountyCampaingTask>(id);
            if (bountyCampaingItem == null || bountyCampaingItem.Id != bountyId)
            {
                return NotFound();
            }

            bountyCampaingItem.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return Ok(bountyCampaingItem);
        }
    }
}