using CWPIO.Data;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.DataProtection
{
    public static class DataProtectionKeyExtensions
    {
        public static IDataProtectionBuilder PersistKeysToSql(this IDataProtectionBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            
            return builder.Use(ServiceDescriptor.Scoped<IXmlRepository, DataProtectionKeyRepository>());
        }

        public static IDataProtectionBuilder Use(this IDataProtectionBuilder builder, ServiceDescriptor descriptor)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            for (int i = builder.Services.Count - 1; i >= 0; i--)
            {
                if (builder.Services[i]?.ServiceType == descriptor.ServiceType)
                {
                    builder.Services.RemoveAt(i);
                }
            }

            builder.Services.Add(descriptor);

            return builder;
        }
    }
}
