using Newtonsoft.Json;

namespace pre_ico_web_site.Models
{
    public class PurchaseRequestModel
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

    public class ExchangeRequestModel : PurchaseRequestModel
    {
        [JsonProperty(PropertyName = "tx")]
        public string Tx { get; set; }
    }
}
