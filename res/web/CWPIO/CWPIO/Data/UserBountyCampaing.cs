using Newtonsoft.Json;
using System.Collections.Generic;

namespace CWPIO.Data
{
    public class UserBountyCampaing
    {
        [JsonIgnore] public string UserId { get; set; }
        [JsonIgnore] public string BountyCampaingId { get; set; }

        public decimal TotalCoinEarned { get; set; }
        public int TotalItemCount { get; set; }
        [JsonIgnore] public virtual ApplicationUser User { get; set; }
        [JsonIgnore] public virtual BountyCampaing BountyCampaing { get; set; }
        public virtual ICollection<UserBountyCampaingItem> Items { get; set; }
    }
}