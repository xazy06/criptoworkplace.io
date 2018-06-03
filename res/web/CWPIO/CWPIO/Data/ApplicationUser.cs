using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Claims = new HashSet<IdentityUserClaim<string>>();
            BountyUserCampaings = new HashSet<BountyUserCampaing>();
            BountyCampaingAcceptedTasks = new HashSet<BountyCampaingAcceptedTask>();
            BountyFavoriteUsers = new HashSet<BountyFavoriteUser>();
        }

        public bool IsDeleted { get; set; }
        public string EthAddress { get; set; }

        [JsonIgnore] public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        [JsonIgnore] public virtual ICollection<BountyUserCampaing> BountyUserCampaings { get; set; }

        [JsonIgnore] public virtual ICollection<BountyCampaingAcceptedTask> BountyCampaingAcceptedTasks { get; set; }
        [JsonIgnore] public virtual ICollection<BountyFavoriteUser> BountyFavoriteUsers { get; set; }

    }
}
