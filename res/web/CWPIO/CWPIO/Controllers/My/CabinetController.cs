using CWPIO.Data;
using CWPIO.Models;
using CWPIO.Models.CabinetViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWPIO.Controllers
{
    [Authorize]
    [Route("my")]
    public class CabinetController : BaseController
    {
        private const string API_KEY = "LIgskaeb32789dsalfnq3eo8dc=[km";

        private readonly ILogger _logger;
        private readonly ISlackClient _slackClient;
        private readonly UserManager<ApplicationUser> _userManager;

        public CabinetController(ApplicationDbContext context, ILogger<CabinetController> logger, ISlackClient slackClient, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _logger = logger;
            _slackClient = slackClient;
            _userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            return RedirectToAction(nameof(ExchangerAsync));
        }

        [HttpGet("faq", Name = "Faq")]
        public IActionResult Faq()
        {

            return View();
        }

        [HttpGet("exchanger", Name = "Exchanger")]
        public async Task<IActionResult> ExchangerAsync(string a)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound();

            if (user.EthAddress == null)
            {
                return RedirectToAction(nameof(WhiteListAsync));
            }

            return View(a == "temp" ? "ExchangerTemp" : "Exchanger");
        }
        
        [HttpGet("whiteList", Name = "WhiteList")]
        public async Task<IActionResult> WhiteListAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound();

            if (user.EthAddress != null)
            {
                return RedirectToAction(nameof(ExchangerAsync));
            }

            return View("WhiteList");
        }
        
        [HttpPost("whiteList", Name = "WhiteList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WhiteListAsync(string ercAddress)
        {
            if (string.IsNullOrEmpty(ercAddress))
                return BadRequest();

            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound();

            if (user.EthAddress != null)
                return BadRequest();

            user.EthAddress = StringToByteArray(ercAddress);
            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(ExchangerAsync));
        }

        [HttpGet("users", Name = "UserManagement")]
        public IActionResult UserManagement()
        {
            
            var model = new UserManagementViewModel() { Users = _userManager.Users.ToList() };
            return View(model);
        }


        private static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("0x", "");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
