using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CWPIO.Data;
using CWPIO.Models.BountyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;

namespace CWPIO.Controllers
{
    [Authorize]
    [Route("my/bounty")]
    public class BountyController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ISlackClient _slackClient;
        private readonly UserManager<ApplicationUser> _userManager;

        public BountyController(ApplicationDbContext context, ILogger<CabinetController> logger, ISlackClient slackClient, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _logger = logger;
            _slackClient = slackClient;
            _userManager = userManager;
        }

        [HttpGet("", Name = "BountyDashboard")]
        public async Task<IActionResult> Index()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return NotFound();
            var user = await DbContext.Users.SingleAsync(u => u.Id == claim.Value);
            return View(user);
        }

        [HttpGet("workplace", Name = "BountyWorkplace")]
        public async Task<IActionResult> Workplace()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound();

            var model = new WorkplaceViewModel
            {
                Bounties = DbContext.BountyCampaings.Include(b => b.Activities),
                MyBounties = DbContext.BountyUserCampaings
                    //.Include(b=> b.Items)
                    .Include(b => b.BountyCampaing.Activities)
                    .Where(b => b.UserId == user.Id)
            };
            return View(model);
        }

        [HttpGet("communication", Name = "BountyCommunication")]
        public async Task<IActionResult> Communication()
        {
            return View();
        }

        [HttpGet("moderation", Name = "BountyModeration")]
        public async Task<IActionResult> Moderation()
        {
            return View();
        }

        [HttpGet("manage", Name = "BountyManage")]
        public IActionResult Manage()
        {
            var model = new BountyManagementViewModel { Bounties = DbContext.Set<BountyCampaing>() };

            return View(model);
        }
    }
}