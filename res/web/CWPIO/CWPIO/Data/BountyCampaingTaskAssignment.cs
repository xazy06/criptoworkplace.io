using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Data
{
    public class BountyCampaingTaskAssignment
    {
        [JsonIgnore] public string AssignedToUserId { get; set; }
        [JsonIgnore] public string BountyCampaingTaskId { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore] public virtual BountyCampaingTask BountyCampaingTask { get; set; }
        [JsonIgnore] public virtual ApplicationUser AssignedToUser { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
