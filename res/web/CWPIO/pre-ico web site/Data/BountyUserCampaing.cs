using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pre_ico_web_site.Data
{
    public class BountyUserCampaing
    {
        [JsonIgnore] public string UserId { get; set; }
        [JsonIgnore] public string BountyCampaingId { get; set; }
        public decimal TotalCoinEarned { get; set; }
        public int TotalItemCount { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        [JsonIgnore] public DateTime DateCreated { get; set; }
        [JsonIgnore] public virtual ApplicationUser User { get; set; }
        [JsonIgnore] public virtual BountyCampaing BountyCampaing { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
    }
}