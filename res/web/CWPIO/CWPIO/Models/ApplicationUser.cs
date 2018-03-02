using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Claims = new HashSet<IdentityUserClaim<string>>();
        }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }
}
