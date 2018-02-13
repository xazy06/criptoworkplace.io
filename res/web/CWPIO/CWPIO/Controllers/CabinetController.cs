using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("")]
        public IActionResult Index()
        {

            return View();
        }
        
    }
}
