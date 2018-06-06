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
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/usersettings")]
    public class UserSettingsController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public UserSettingsController(ApplicationDbContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: api/v1/usersettings
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new SimpleApplicationUserSettingsDto { EthAddress = user.EthAddress });

        }

        // PUT: api/v1/usersettings
        [HttpPut()]
        public async Task<IActionResult> PutAsync([FromBody] SimpleApplicationUserSettingsDto userSettings)
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

            if (!string.IsNullOrEmpty(userSettings.EthAddress))
                user.EthAddress = userSettings.EthAddress;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await UserExistsAsync(user.Id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(userSettings);
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            return await _dbContext.Users.AnyAsync(e => e.Id == id);
        }
    }
}