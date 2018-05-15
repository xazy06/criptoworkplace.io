using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Data
{
    public class BountyCampaingAcceptedTask
    {
        [JsonIgnore] public string AcceptedByUserId { get; set; }
        [JsonIgnore] public string BountyCampaingTaskId { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        public BountyCampaingTaskStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
        public int? BlobOid { get; set; }
        [JsonIgnore] public virtual ApplicationUser AcceptedByUser { get; set; }
        [JsonIgnore] public virtual BountyCampaingTask BountyCampaingTask { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
