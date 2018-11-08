using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pre_ico_web_site.Data;
using System;

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
                var dbContext = services.GetService<DataProtectionDbContext>() ?? new DataProtectionDbContext(new EntityFrameworkCore.DbContextOptions<DataProtectionDbContext>(), services.GetService<ILogger<DataProtectionDbContext>>());
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new DataProtectionKeyRepository(dbContext);
                });
            });

            return builder;
        }
    }
}
