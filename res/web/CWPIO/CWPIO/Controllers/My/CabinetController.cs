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
    public class CabinetController : Controller
    {
        private const string API_KEY = "LIgskaeb32789dsalfnq3eo8dc=[km";

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly ISlackClient _slackClient;
        private readonly UserManager<ApplicationUser> _userManager;

        public CabinetController(ApplicationDbContext context, ILogger<CabinetController> logger, ISlackClient slackClient, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _slackClient = slackClient;
            _userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            return View();
        }
        
        [HttpGet("users", Name = "UserManagement")]
        public IActionResult UserManagement()
        {
            
            var model = new UserManagementViewModel() { Users = _userManager.Users.ToList() };
            return View(model);
        }

        
    }
}
