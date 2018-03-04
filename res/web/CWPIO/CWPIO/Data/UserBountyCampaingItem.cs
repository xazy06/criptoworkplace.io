namespace CWPIO.Data
{
    public class UserBountyCampaingItem
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BountyCampaingId { get; set; }
        public string Url { get; set; }
        public bool? IsAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public int ItemType { get; set; }
        public virtual UserBountyCampaing UserBounty { get; set; }
    }
}