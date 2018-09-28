using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Data
{
    public class BountyCampaingTask
    {
        public BountyCampaingTask()
        {
            BountyCampaingTaskAssignments = new HashSet<BountyCampaingTaskAssignment>();
        }

        [JsonIgnore] public string Id { get; set; }
        [JsonIgnore] public string BountyCampaingId { get; set; }
        [JsonIgnore] public string BountyCampaingActivityId { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore] public virtual BountyCampaing BountyCampaing { get; set; }
        [JsonIgnore] public virtual BountyCampaingActivity BountyCampaingActivity { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
        [JsonIgnore] public virtual ICollection<BountyCampaingTaskAssignment> BountyCampaingTaskAssignments { get; set; }
    }
}
