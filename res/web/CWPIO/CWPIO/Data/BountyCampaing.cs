using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CWPIO.Data
{
    public class BountyCampaing
    {
        public string Id { get; set; }
        [Required] public string Name { get; set; }
        public string FaClass { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        [JsonIgnore] public DateTime DateCreated { get; set; }
        public virtual ICollection<BountyCampaingActivity> Activities { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }

        public Dictionary<string, decimal> ActivityPrices => Activities?.Where(x => !x.IsDeleted)?.ToDictionary(x => x.Id, x => x.Price);
    }
}