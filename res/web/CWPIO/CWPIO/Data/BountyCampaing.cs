using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CWPIO.Data
{
    public class BountyCampaing
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string FaClass { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<UserBountyCampaing> UserBounties { get; set; }
        public virtual ICollection<BountyCampaingItemType> ItemTypes { get; set; }

        public Dictionary<int, decimal> Prices => ItemTypes.Where(x => !x.IsDeleted).ToDictionary(x => x.TypeId, x => x.Price);
    }
}