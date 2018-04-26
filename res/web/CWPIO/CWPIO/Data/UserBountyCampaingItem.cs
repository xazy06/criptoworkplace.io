using Newtonsoft.Json;

namespace CWPIO.Data
{
    public class UserBountyCampaingItem
    {
        [JsonIgnore] public string Id { get; set; }
        [JsonIgnore] public string UserId { get; set; }
        [JsonIgnore] public string BountyCampaingId { get; set; }
        public string Url { get; set; }
        public bool? IsAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public int ItemType { get; set; }
        [JsonIgnore] public virtual UserBountyCampaing UserBounty { get; set; }
    }
}