using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pre_ico_web_site.Models;
using pre_ico_web_site.Data;
using Microsoft.Extensions.Logging;

namespace pre_ico_web_site.Data
{
    public class DataProtectionDbContext : DbContext
    {
        public DataProtectionDbContext(DbContextOptions<DataProtectionDbContext> options, ILogger<DataProtectionDbContext> logger)
            : base(options)
        {
            logger?.LogInformation($"Data Protection Using database: {Database.GetDbConnection().Database}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DataProtectionKey>(b =>
            {
                b.ToTable("data_protection_keys","core");

                b.HasKey(x => x.FriendlyName);
                b.Property(p => p.FriendlyName).HasColumnName("FriendlyName").HasColumnType("text");
                b.Property(p => p.XmlData).HasColumnName("XmlData").HasColumnType("text");
            });
            
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.ToSnakeCase();
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                }
            }
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    }
}
