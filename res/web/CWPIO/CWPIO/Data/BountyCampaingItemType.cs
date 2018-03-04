namespace CWPIO.Data
{
    public class BountyCampaingItemType
    {
        public string Id { get; set; }
        public int TypeId { get; set; }
        public string BountyCampaingId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool NeedToApprove { get; set; }
        public bool IsDeleted { get; set; }

        public virtual BountyCampaing BountyCampaing { get; set; }
    }
}