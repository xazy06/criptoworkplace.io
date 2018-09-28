using Newtonsoft.Json;
using System;

namespace pre_ico_web_site.Data
{
    public class BountyCampaingActivity
    {
        public string Id { get; set; }
        [JsonIgnore] public string BountyCampaingId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool NeedToApprove { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        [JsonIgnore] public DateTime DateCreated { get; set; }
        [JsonIgnore] public virtual BountyCampaing BountyCampaing { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
    }
}