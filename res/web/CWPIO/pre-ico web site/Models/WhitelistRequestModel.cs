using Newtonsoft.Json;

namespace pre_ico_web_site.Models
{
    public class WhitelistRequestModel
    {
        [JsonProperty(PropertyName = "ercAddress")]
        public string ErcAddress { get; set; }
    }
}
