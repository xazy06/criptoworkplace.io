using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private const string API_KEY = "LIgskaeb32789dsalfnq3eo8dc=[km";

        public IActionResult Index()
        {

            return View();
        }
        
    }
}
