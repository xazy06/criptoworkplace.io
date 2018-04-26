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
    [Route("api/v1/bounty/{bountyId}/items")]
    public class BountyItemsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BountyItemsController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute]string bountyId, [FromQuery]bool includeDeleted = false)
        {
            var result = _dbContext
                .BountiesItemTypes
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
            var result = await _dbContext.FindAsync<BountyCampaingItemType>(id);

            if (result == null || result.BountyCampaingId != bountyId)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromRoute]string bountyId, [FromBody]SimpleBountyItemDto bountyItemType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await _dbContext.FindAsync<BountyCampaing>(bountyId);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            await _dbContext.Entry(bountyCampaing).Collection(b => b.ItemTypes).LoadAsync();

            var newTypeId = bountyCampaing.ItemTypes.Any() ? (bountyCampaing.ItemTypes.Max(i => i.TypeId) + 1) : 1;
            var newBountyItem = new BountyCampaingItemType
            {
                TypeId = newTypeId,
                Name = bountyItemType.Name,
                NeedToApprove = bountyItemType.NeedToApprove,
                Price = bountyItemType.Price.Value
            };
            bountyCampaing.ItemTypes.Add(newBountyItem);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAsync", new { id = newBountyItem.Id }, newBountyItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute]string bountyId, [FromRoute]string id, [FromBody]SimpleBountyItemDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyItemType = _dbContext.Find<BountyCampaingItemType>(id);

            if (bountyItemType == null || bountyItemType.BountyCampaingId != bountyId)
            {
                return NotFound();
            }

            bountyItemType.Name = value.Name;
            bountyItemType.Price = value.Price.Value;
            bountyItemType.NeedToApprove = value.NeedToApprove;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await BountyCampaingItemTypeExistsAsync(bountyId, id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]string bountyId, [FromRoute]string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaingItemType = await _dbContext.FindAsync<BountyCampaingItemType>(id);
            if (bountyCampaingItemType == null || bountyCampaingItemType.Id != bountyId)
            {
                return NotFound();
            }

            //_dbContext.Remove(bountyCampaing);
            bountyCampaingItemType.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return Ok(bountyCampaingItemType);
        }

        private async Task<bool> BountyCampaingItemTypeExistsAsync(string bountyId, string id)
        {
            return await _dbContext.BountiesItemTypes.AnyAsync(e => e.BountyCampaingId == bountyId && e.Id == id);
        }
    }
}