using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CWPIO.Data;

namespace CWPIO.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/bounty")]
    public class BountyController : BaseController
    {

        public BountyController(ApplicationDbContext context)
            : base(context)
        {
        }

        // GET: api/Bounty
        [HttpGet]
        public IEnumerable<BountyCampaing> GetBountyCampaing()
        {
            return DbContext.Bounties;
        }

        // GET: api/Bounty/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBountyCampaing([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await DbContext.Bounties.SingleOrDefaultAsync(m => m.Id == id);

            if (bountyCampaing == null)
            {
                return NotFound();
            }

            return Ok(bountyCampaing);
        }

        // PUT: api/Bounty/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBountyCampaing([FromRoute] string id, [FromBody] BountyCampaing bountyCampaing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bountyCampaing.Id)
            {
                return BadRequest();
            }

            DbContext.Entry(bountyCampaing).State = EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BountyCampaingExists(id))
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

        // POST: api/Bounty
        [HttpPost]
        public async Task<IActionResult> PostBountyCampaing([FromBody] BountyCampaing bountyCampaing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DbContext.Bounties.Add(bountyCampaing);
            await DbContext.SaveChangesAsync();

            return CreatedAtAction("GetBountyCampaing", new { id = bountyCampaing.Id }, bountyCampaing);
        }

        // DELETE: api/Bounty/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBountyCampaing([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await DbContext.Bounties.SingleOrDefaultAsync(m => m.Id == id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            DbContext.Bounties.Remove(bountyCampaing);
            await DbContext.SaveChangesAsync();

            return Ok(bountyCampaing);
        }

        private bool BountyCampaingExists(string id)
        {
            return DbContext.Bounties.Any(e => e.Id == id);
        }

        // POST: api/Bounty/5/participate
        [HttpPost("{id}/participate")]
        public async Task<IActionResult> Participate([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            var bountyCampaing = await DbContext.Bounties.Include(b => b.UserBounties).SingleOrDefaultAsync(m => m.Id == id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            if (bountyCampaing.UserBounties.Any(b => b.UserId == user.Id))
            {
                return BadRequest(new { error = "Already participate in this bounty program" });
            }

            var item = new UserBountyCampaing { User = user };
            bountyCampaing.UserBounties.Add(item);

            await DbContext.SaveChangesAsync();

            return Ok(item);
        }

        // GET: api/bounty/5/itemtype
        [HttpGet("{id}/itemtype")]
        public async Task<IActionResult> GetBountyItemType([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await DbContext.Bounties.Include(x => x.ItemTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            return Ok(bountyCampaing.ItemTypes);
        }

        // GET: api/bounty/5/itemtype
        [HttpGet("{id}/itemtype/{itemId}")]
        public async Task<IActionResult> GetBountyItemType([FromRoute] string id, [FromRoute] int itemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await DbContext.Bounties.Include(x => x.ItemTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }
            var itemType = bountyCampaing.ItemTypes.SingleOrDefault(m => m.TypeId == itemId);
            if (itemType == null)
            {
                return NotFound();
            }
            return Ok(itemType);
        }

        // POST: api/Bounty/5/itemtype
        [HttpPost("{id}/itemtype")]
        public async Task<IActionResult> AddItemType([FromRoute] string id, [FromBody] BountyCampaingItemType itemType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await DbContext.Bounties.Include(b => b.UserBounties).SingleOrDefaultAsync(m => m.Id == id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            if (bountyCampaing.ItemTypes.Any(b => !b.IsDeleted && b.TypeId == itemType.TypeId))
            {
                return BadRequest(new { error = $"Already has item type {itemType.TypeId}" });
            }

            bountyCampaing.ItemTypes.Add(itemType);

            await DbContext.SaveChangesAsync();

            return Ok(itemType);
        }

        // POST: api/bounty/5/register
        [HttpPost("{id}/register")]
        public async Task<IActionResult> Register([FromRoute] string id, [FromBody]UserBountyCampaingItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            var bountyCampaing = await DbContext.UserBounties
                .Include(b => b.Items)
                .SingleOrDefaultAsync(m => m.BountyCampaingId == id && m.UserId == user.Id && !m.BountyCampaing.IsDeleted);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            if (DbContext.UserBountyItems.Any(b => b.Url == item.Url))
            {
                return BadRequest(new { error = "Already register this url" });
            }

            
            bountyCampaing.Items.Add(item);
            bountyCampaing.TotalItemCount = bountyCampaing.Items
                .Where(i => i.IsAccepted.HasValue && i.IsAccepted.Value && !i.IsDeleted)
                .Count();

            bountyCampaing.TotalCoinEarned = bountyCampaing.Items
                .Where(i => i.IsAccepted.HasValue && i.IsAccepted.Value && !i.IsDeleted)
                .Sum(i => bountyCampaing.BountyCampaing.Prices[i.ItemType]);

            await DbContext.SaveChangesAsync();

            return Ok(item);
        }
    }
}