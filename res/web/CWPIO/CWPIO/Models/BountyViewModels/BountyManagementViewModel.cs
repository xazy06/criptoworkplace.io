using CWPIO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Models.BountyViewModels
{
    public class BountyManagementViewModel
    {
        public IEnumerable<BountyCampaing> Bounties { get; set; }
    }
}
