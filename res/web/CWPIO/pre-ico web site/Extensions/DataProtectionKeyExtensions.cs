using CWPIO.Data;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            {
                var dbContext = services.GetService<ApplicationDbContext>() ?? new ApplicationDbContext(new EntityFrameworkCore.DbContextOptions<ApplicationDbContext>());
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new DataProtectionKeyRepository(dbContext);
                });
            });

            return builder;
        }
    }
}
