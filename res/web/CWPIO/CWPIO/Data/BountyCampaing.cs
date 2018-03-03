using System.Collections.Generic;

namespace CWPIO.Data
{
    public class BountyCampaing
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserBountyCampaing> UserBounties { get; set; }
    }
}