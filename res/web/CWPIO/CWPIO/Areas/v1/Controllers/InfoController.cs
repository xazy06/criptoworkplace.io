using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CWPIO.Areas.v1.Controllers
{
    [Area("v1")]
    [Route("api/v1")]
    public class InfoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}