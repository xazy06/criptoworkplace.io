using CWPIO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Models.BountyViewModels
{
    public class WorkplaceViewModel
    {
        public IEnumerable<BountyCampaing> Bounties { get; set; }
        public IEnumerable<UserBountyCampaing> MyBounties { get; set; }
    }
}
