using CWPIO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWPIO.Models.CabinetViewModels
{
    public class UserManagementViewModel
    {
        public List<ApplicationUser> Users { get; set; }
    }
}
