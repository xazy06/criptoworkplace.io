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
        private const string API_KEY = "LIgskaeb32789dsalfnq3eo8dc=[km";

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ISlackClient _slackClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenSaleContract _tokenSaleContract;

        public CabinetController(
            ApplicationDbContext context,
            ILogger<CabinetController> logger,
            ISlackClient slackClient,
            UserManager<ApplicationUser> userManager,
            TokenSaleContract tokenSaleContract
            )
        {
            _dbContext = context;
            _logger = logger;
            _slackClient = slackClient;
            _userManager = userManager;
            _tokenSaleContract = tokenSaleContract;
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
            {
                return NotFound();
            }

            if (user.EthAddress == null)
            {
                return RedirectToAction(nameof(WhiteListAsync));
            }

            var model = new BarModel
            {
                Sold = (int)UnitConversion.Convert.FromWei(await _tokenSaleContract.TokenSoldAsync()),
                Cap = (int)UnitConversion.Convert.FromWei(await _tokenSaleContract.GetCapAsync())
            };

            return View(a == "temp" ? "ExchangerTemp" : "Exchanger", model);
        }

        [HttpGet("whiteList", Name = "WhiteList")]
        public async Task<IActionResult> WhiteListAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            if (user.EthAddress != null)
            {
                return RedirectToAction(nameof(ExchangerAsync));
            }

            return View("WhiteList");
        }

        [HttpGet("kyc", Name = "KYC")]
        public IActionResult Kyc()
        {

            return View();
        }

        [HttpPost("whiteList", Name = "WhiteList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WhiteListAsync(string ercAddress)
        {
            if (string.IsNullOrEmpty(ercAddress))
            {
                return BadRequest();
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            if (user.EthAddress != null)
            {
                return BadRequest();
            }

            await _tokenSaleContract.AddAddressToWhitelistAsync(ercAddress);

            user.EthAddress = StringToByteArray(ercAddress);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(ExchangerAsync));
        }

        private static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("0x", "");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }

            var user = await _dbContext.Users.FindAsync(claim.Value);
            return user;
        }
    }
}
