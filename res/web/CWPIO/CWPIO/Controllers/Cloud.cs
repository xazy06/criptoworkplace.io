using Microsoft.AspNetCore.Mvc;

namespace CWPIO.Controllers
{
    public class Cloud : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}