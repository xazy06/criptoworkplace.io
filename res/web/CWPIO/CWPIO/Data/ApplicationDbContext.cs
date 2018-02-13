using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CWPIO.Models;

namespace CWPIO.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Subscriber>(b =>
            {
                b.ToTable("Subscribers");

                b.HasKey(s => s.Id);
                b.Property(s => s.Name).IsRequired().HasMaxLength(100);
                b.Property(s => s.Email).IsRequired().HasMaxLength(100);
                b.Property(s => s.EmailSend).IsRequired().HasDefaultValue(false);
            });

            builder.Entity<DataProtectionKey>(b => {
                b.ToTable("DataProtectionKeys");

                b.HasKey(x => x.FriendlyName);
                b.Property(p => p.FriendlyName).HasColumnName("FriendlyName").HasColumnType("nvarchar(max)");
                b.Property(p => p.XmlData).HasColumnName("XmlData").HasColumnType("nvarchar(max)");
            });
        }
    }
}
