using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWPIO.Areas.v1.Models;
using CWPIO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CWPIO.Areas.v1.Controllers
{
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/bounty")]
    public class BountyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BountyController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Get list of all bounties
        /// GET: api/v1/bounty?includeDeleted=0
        /// </summary>
        /// <param name="includeDeleted">Include deleted bounty in response</param>
        /// <returns>List of <see cref="BountyCampaing"/> active campaings </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]bool includeDeleted = false)
        {
            var result = _dbContext.BountyCampaings
                .Include(b => b.Activities)
                .AsQueryable();
            if (!includeDeleted)
                result = result.Where(b => !b.IsDeleted);
            if (await result.AnyAsync())
                return Ok(await result.ToListAsync());

            return NotFound();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _dbContext.FindAsync<BountyCampaing>(id);

            if (result == null)
            {
                return NotFound();
            }

            await _dbContext.Entry(result).Collection(b => b.Activities).LoadAsync();

            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SimpleBountyDto bounty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
                return NotFound();

            var bountyCampaing = _dbContext.BountyCampaings.Add(new BountyCampaing() { Name = bounty.Name, FaClass = bounty.FaClass, CreatedByUser = user });
            await bountyCampaing.Collection(b => b.Activities).LoadAsync();
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAsync", new { id = bountyCampaing.Entity.Id }, bountyCampaing.Entity);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute]string id, [FromBody]SimpleBountyDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bounty = _dbContext.Find<BountyCampaing>(id);

            if (bounty == null)
            {
                return NotFound();
            }

            bounty.Name = value.Name;
            bounty.FaClass = value.FaClass;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await BountyCampaingExistsAsync(id)))
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
        public async Task<IActionResult> DeleteAsync([FromRoute]string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bountyCampaing = await _dbContext.FindAsync<BountyCampaing>(id);
            if (bountyCampaing == null)
            {
                return NotFound();
            }

            //_dbContext.Remove(bountyCampaing);
            bountyCampaing.IsDeleted = true;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await BountyCampaingExistsAsync(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(bountyCampaing);
        }

        private async Task<bool> BountyCampaingExistsAsync(string id)
        {
            return await _dbContext.BountyCampaings.AnyAsync(e => e.Id == id);
        }
    }
}
