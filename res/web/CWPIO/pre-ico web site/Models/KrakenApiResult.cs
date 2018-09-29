using Newtonsoft.Json;
using System.Collections.Generic;

namespace pre_ico_web_site.Models
{
    public class KrakenApiResult<T>
    {
        public List<string> Error { get; set; }
        public T Result { get; set; }

    }

    public class KrakenEthUsdTick
    {
        public KrakenPairTick XETHZUSD { get; set; }

    }

    public class KrakenPairTick
    {
        [JsonProperty(PropertyName = "c")]
        public List<string> Current { get; set; }
    }
}

