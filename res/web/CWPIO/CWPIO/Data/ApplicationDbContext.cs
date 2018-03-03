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

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Subscriber>(b =>
            {
                b.ToTable("Subscribers");

                b.HasKey(s => s.Id);

                b.Property(s => s.Name).IsRequired().HasMaxLength(100);
                b.Property(s => s.Email).IsRequired().HasMaxLength(100);
                b.Property(s => s.EmailSend).IsRequired();
                b.Property(s => s.Culture).IsRequired().HasDefaultValue("");
            });

            builder.Entity<DataProtectionKey>(b =>
            {
                b.ToTable("DataProtectionKeys");

                b.HasKey(x => x.FriendlyName);
                b.Property(p => p.FriendlyName).HasColumnName("FriendlyName").HasColumnType("text");
                b.Property(p => p.XmlData).HasColumnName("XmlData").HasColumnType("text");
            });

            builder.Entity<BountyCampaing>(b =>
            {
                b.ToTable("BountyCampaing");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            });

            builder.Entity<UserBountyCampaing>(b =>
            {
                b.ToTable("UserBountyCampaing");

                b.HasKey(x => new { x.UserId, x.BountyCampaingId });
                b.Property(x => x.TotalItemCount).IsRequired(true).HasDefaultValue(0);
                b.Property(x => x.TotalCoinEarned).IsRequired(true).HasDefaultValue(0m);

                b.HasOne(x => x.BountyCampaing)
                    .WithMany(x => x.UserBounties)
                    .HasForeignKey(x => x.BountyCampaingId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.User)
                    .WithMany(x => x.UserBounties)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
