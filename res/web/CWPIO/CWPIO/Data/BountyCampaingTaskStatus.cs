using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Data
{
    public enum BountyCampaingTaskStatus:int
    {
        InWork = 0,
        Moderation = 1,
        Abandon = 2,
        Completed = 3
    }
}
