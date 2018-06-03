using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CWPIO.Data;
using CWPIO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CWPIO.Areas.v1.Models;

namespace CWPIO.Areas.v1.Controllers
{
    [Authorize(Policy = "CanAccessUsers")]
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/user")]
    public class UserController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public UserController(ApplicationDbContext dbContext, /*UserManager<ApplicationUser> userManager,*/ ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            //_userManager = userManager;
            _logger = logger;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]bool includeDeleted = false)
        {
            var result = _dbContext
                .Users
                //.Include(u => u.Claims)
                .AsQueryable();

            if (!includeDeleted)
                result = result.Where(u => !u.IsDeleted);
            if (await result.AnyAsync())
                return Ok(await result.ToListAsync());

            return NotFound();
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _dbContext.FindAsync<ApplicationUser>(id);

            if (applicationUser == null)
            {
                return NotFound();
            }
            //await _dbContext.Entry(applicationUser).Collection(b => b.Claims).LoadAsync();
            return Ok(applicationUser);
        }

        //// GET: api/user/claims/5
        //[HttpGet("claims/{id}")]
        //public async Task<IActionResult> GetApplicationUserClaims([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var applicationUser = await _userManager.FindByIdAsync(id);

        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

        //    var claims = await _userManager.GetClaimsAsync(applicationUser);
        //    return Ok(claims.Select(c => new { c.Type, c.Value, c.Issuer }));
        //}

        // POST: api/user
        [Authorize(Policy = "CanEditUsers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] SimpleApplicationUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _dbContext.FindAsync<ApplicationUser>(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(user.Email))
                applicationUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.EthAddress))
                applicationUser.EthAddress = user.EthAddress;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await UserExistsAsync(id)))
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

        //// POST: api/user
        //[Authorize(Policy = "CanEditUsers")]
        //[HttpPost]
        //public async Task<IActionResult> PostAsync([FromBody] SimpleApplicationUserDto user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var applicationUser = new ApplicationUser { UserName = user.Email, Email = user.Email };

        //    var result = await _userManager.CreateAsync(applicationUser);
        //    if (!result.Succeeded)
        //        return NotFound();
        //    else
        //        foreach (var err in result.Errors)
        //            _logger.LogError($"{err.Code}:{err.Description}");


        //    return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        //}

        //// PUT: api/user/5
        //[Authorize(Policy = "CanEditUsers")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutApplicationUser([FromRoute] string id, [FromBody] SimpleApplicationUserDto user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _userManager.UpdateAsync(applicationUser);
        //    if (!result.Succeeded)
        //        return NotFound();
        //    else
        //        foreach (var err in result.Errors)
        //            _logger.LogError($"{err.Code}:{err.Description}");

        //    return NoContent();
        //}



        // DELETE: api/user/5
        [Authorize(Policy = "CanEditUsers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.FindAsync<ApplicationUser>(id);
            if (user == null)
                return NotFound();

            user.IsDeleted = true;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await UserExistsAsync(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(user);
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            return await _dbContext.Users.AnyAsync(e => e.Id == id);
        }
    }
}