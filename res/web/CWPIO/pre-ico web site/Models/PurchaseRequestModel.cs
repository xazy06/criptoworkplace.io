using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class PurchaseRequestModel
    {
        [JsonProperty(PropertyName="count")]
        public int Count { get; set; }
    }
}
