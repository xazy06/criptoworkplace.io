using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nethereum.Util;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using Slack.Webhooks;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    [Authorize]
    [Route("my")]
    public class CabinetController : Controller
    {

        [HttpGet("exchanger", Name = "Exchanger")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("Exchanger");
        }

        [HttpGet("faq", Name = "Faq")]
        public IActionResult Faq()
        {
            return View();
        }
        
        [HttpGet("kyc", Name = "KYC")]
        public IActionResult Kyc()
        {
            return View();
        }      

        
    }
}
