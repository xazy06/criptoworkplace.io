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
            if(code == 404) return View("404", "Error");
            
            if(code == 500) return View("500", "Error");
            
            return View(code);
        }
    }
}