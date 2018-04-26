using Newtonsoft.Json;

namespace CWPIO.Data
{
    public class BountyCampaingItemType
    {
        public string Id { get; set; }
        public int TypeId { get; set; }
        [JsonIgnore]
        public string BountyCampaingId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool NeedToApprove { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual BountyCampaing BountyCampaing { get; set; }
    }
}