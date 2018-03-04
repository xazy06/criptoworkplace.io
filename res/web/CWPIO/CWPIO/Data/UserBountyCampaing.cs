using System.Collections.Generic;

namespace CWPIO.Data
{
    public class UserBountyCampaing
    {
        public string UserId { get; set; }
        public string BountyCampaingId { get; set; }

        public decimal TotalCoinEarned { get; set; }
        public int TotalItemCount { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual BountyCampaing BountyCampaing { get; set; }
        public virtual ICollection<UserBountyCampaingItem> Items { get; set; }
    }
}