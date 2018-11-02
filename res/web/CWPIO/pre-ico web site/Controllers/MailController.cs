using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pre_ico_web_site.Data;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System.IO;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/sendMail")]
    public class MailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;

        public MailController(IEmailSender emailSender, ApplicationDbContext dbContext)
        {
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostMail([FromBody]SendMailModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _emailSender.SendEmailFailedTransactionAsync(user.Email, model.Body);

            return Ok();
        }
    }
}