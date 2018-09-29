using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public class RateStore : IRateStore
    {
        private RateStoreSettings _options;
        private IMemoryCache _memoryCache;
        private readonly string entry = "rate";

        public RateStore(IOptions<RateStoreSettings> options, IMemoryCache memoryCache)
        {
            _options = options.Value;
            _memoryCache = memoryCache;
        }

        public Task<decimal> GetRateAsync()
        {
            return _memoryCache.GetOrCreateAsync<decimal>(entry, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                // Key not in cache, so get data.
                var json = await new HttpClient().GetStringAsync(_options.KrakenApiUri);
                var obj = JsonConvert.DeserializeObject<KrakenApiResult<KrakenEthUsdTick>>(json);
                if (decimal.TryParse(obj.Result.XETHZUSD.Current[0], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out decimal cacheEntry))
                {
                    return cacheEntry;
                }
                else
                {
                    json = await new HttpClient().GetStringAsync(_options.CoinMarketCapApiUri);
                    var obj2 = JsonConvert.DeserializeObject<List<CoinmarketResult>>(json).FirstOrDefault();
                    if (decimal.TryParse(obj2.PriceUsd, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out cacheEntry))
                    {
                        return cacheEntry;
                    }
                    else
                    {
                        return 240.0m;
                    }
                }
            });

        }
    }
}