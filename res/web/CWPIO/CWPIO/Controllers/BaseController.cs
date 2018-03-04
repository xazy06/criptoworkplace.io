using CWPIO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWPIO.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext DbContext;
        public BaseController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return null;
            var user = await DbContext.Users.FindAsync(claim.Value);
            return user;
        }
    }
}
