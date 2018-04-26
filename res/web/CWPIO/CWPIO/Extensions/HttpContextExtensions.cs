using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetCurrentCulture(this HttpContext context)
        {
            var rqf = context.Features.Get<IRequestCultureFeature>();

            return rqf?.RequestCulture.Culture.Name ?? "en-US";
        }
    }
}
