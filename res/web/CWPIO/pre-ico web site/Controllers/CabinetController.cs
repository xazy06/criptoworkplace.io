﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pre_ico_web_site.Data;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    [Authorize]
    [Route("my")]
    public class CabinetController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CabinetController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("exchanger", Name = "Exchanger")]
        [HttpGet]
        public async Task<IActionResult> Exchanger()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user.EmailConfirmed)
            {
                return View();

            }
            else
            {
                return RedirectToAction(nameof(AccountController.EmailConfirm), "Account");
            }
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
