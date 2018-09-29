using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class CoinmarketResult
    {
        [JsonProperty(PropertyName = "price_usd")]
        public string PriceUsd { get; set; }
    }
}
