using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CWPIO.Models;
using CWPIO.Data;


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

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);

                b.HasMany(u => u.Claims)
                    .WithOne()
                    .HasForeignKey(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });


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
                b.Property(x => x.IsDeleted).IsRequired(true);
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.FaClass).IsRequired(false).HasMaxLength(100);
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

            builder.Entity<UserBountyCampaingItem>(b =>
            {
                b.ToTable("UserBountyCampaingItem");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.IsAccepted).IsRequired(false);
                b.Property(x => x.IsDeleted).IsRequired(true);
                b.Property(x => x.Url).IsRequired(true).HasMaxLength(255);
                b.Property(x => x.ItemType).IsRequired(true);

                b.HasOne<BountyCampaingItemType>()
                    .WithMany()
                    .HasForeignKey(c => c.ItemType)
                    .HasPrincipalKey(p => p.TypeId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.UserBounty)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => new { x.UserId, x.BountyCampaingId })
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<BountyCampaingItemType>(b =>
            {
                b.ToTable("BountyCampaingItemType");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
                b.Property(x => x.TypeId).IsRequired(true);
                b.Property(x => x.Price).IsRequired(true).HasDefaultValue(0m);
                b.Property(x => x.NeedToApprove).IsRequired(true);
                b.Property(x => x.IsDeleted).IsRequired(true);

                b.HasOne(x => x.BountyCampaing)
                    .WithMany(x => x.ItemTypes)
                    .HasForeignKey(x => x.BountyCampaingId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<BountyCampaing> Bounties { get; set; }
        public DbSet<UserBountyCampaing> UserBounties { get; set; }
        public DbSet<BountyCampaingItemType> BountiesItemTypes { get; set; }
        public DbSet<UserBountyCampaingItem> UserBountyItems { get; set; }
    }
}
