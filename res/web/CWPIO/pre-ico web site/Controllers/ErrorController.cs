using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pre_ico_web_site.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [HttpGet("{code}")]
        public IActionResult Index([FromRoute]int code)
        {
            return View(code);
        }
    }
}