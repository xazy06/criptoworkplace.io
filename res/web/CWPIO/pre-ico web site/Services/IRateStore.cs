using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public interface IRateStore
    {
        Task<decimal> GetRateAsync();
    }
}
