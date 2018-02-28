using CWPIO.Data;
using CWPIO.Models;
using CWPIO.Models.CabinetViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Controllers
{
    [Authorize]
    [Route("my")]
    public class CabinetController : Controller
    {
        private const string API_KEY = "LIgskaeb32789dsalfnq3eo8dc=[km";

        private ApplicationDbContext _context;
        private ILogger _logger;
        private ISlackClient _slackClient;

        public CabinetController(ApplicationDbContext context, ILogger<CabinetController> logger, ISlackClient slackClient)
        {
            _context = context;
            _logger = logger;
            _slackClient = slackClient;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            return View();
        }
        
        [HttpGet("users", Name = "UserManagement")]
        public IActionResult UserManagement()
        {
            var model = new UserManagementViewModel() { Users = _context.Users.Include(u => u.Claims).ToList() };
            return View(model);
        }
    }
}
