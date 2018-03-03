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

namespace CWPIO.Controllers.Api
{
    [Authorize(Policy = "CanAccessUsers")]
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserController(/*ApplicationDbContext context*/UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
        {
            //_context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: api/user
        [HttpGet]
        public IEnumerable<ApplicationUser> GetApplicationUser()
        {
            return _userManager.Users;
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        // GET: api/user/claims/5
        [HttpGet("claims/{id}")]
        public async Task<IActionResult> GetApplicationUserClaims([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(applicationUser);
            return Ok(claims.Select(c => new { c.Type, c.Value, c.Issuer }));
        }

        // PUT: api/user/5
        [Authorize(Policy = "CanEditUsers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser([FromRoute] string id, [FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            var result = await _userManager.UpdateAsync(applicationUser);
            if (!result.Succeeded)
                return NotFound();
            else
                foreach (var err in result.Errors)
                    _logger.LogError($"{err.Code}:{err.Description}");

            return NoContent();
        }

        // POST: api/user
        [Authorize(Policy = "CanEditUsers")]
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser([FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(applicationUser);
            if (!result.Succeeded)
                return NotFound();
            else
                foreach (var err in result.Errors)
                    _logger.LogError($"{err.Code}:{err.Description}");


            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/user/5
        [Authorize(Policy = "CanEditUsers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(applicationUser);
            if (!result.Succeeded)
                return NotFound();
            else
                foreach (var err in result.Errors)
                    _logger.LogError($"{err.Code}:{err.Description}");

            return Ok(applicationUser);
        }
    }
}