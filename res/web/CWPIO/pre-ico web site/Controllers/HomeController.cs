using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Util;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;

namespace pre_ico_web_site.Controllers
{
    public class HomeController : Controller
    {
        private readonly TokenSaleContract _contract;
        public HomeController(TokenSaleContract contract)
        {
            _contract = contract;
        }

        public async Task<IActionResult> Index()
        {
            var model = new BarModel
            {
                Sold = (int)UnitConversion.Convert.FromWei(await _contract.TokenSoldAsync()),
                Cap = (int)UnitConversion.Convert.FromWei(await _contract.GetCapAsync())
            };
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}