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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            logger?.LogInformation($"Using database: {Database.GetDbConnection().Database}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("users", "identity");

                b.Property(u => u.IsDeleted).IsRequired();
                b.Property(u => u.IsDemo).IsRequired().HasDefaultValue(false);

                b.Property(x => x.EthAddress).IsRequired(false).HasMaxLength(20);

                b.HasMany(u => u.Claims)
                    .WithOne()
                    .HasForeignKey(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(b =>
            {
                b.ToTable("roles", "identity");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("role_claims", "identity");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(b =>
            {
                b.ToTable("user_claims", "identity");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(b =>
            {
                b.ToTable("user_logins", "identity");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(b =>
            {
                b.ToTable("user_roles", "identity");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(b =>
            {
                b.ToTable("user_tokens", "identity");
            });

            builder.Entity<Subscriber>(b =>
            {
                b.ToTable("subscribers");

                b.HasKey(s => s.Id);

                b.Property(s => s.Name).IsRequired().HasMaxLength(100);
                b.Property(s => s.Email).IsRequired().HasMaxLength(100);
                b.Property(s => s.EmailSend).IsRequired();
                b.Property(s => s.Culture).IsRequired().HasDefaultValue("");
            });

            builder.Entity<DataProtectionKey>(b =>
            {
                b.ToTable("data_protection_keys","core");

                b.HasKey(x => x.FriendlyName);
                b.Property(p => p.FriendlyName).HasColumnName("FriendlyName").HasColumnType("text");
                b.Property(p => p.XmlData).HasColumnName("XmlData").HasColumnType("text");
            });

            builder.Entity<BountyCampaing>(b =>
            {
                b.ToTable("campaing", "bounty");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.IsDeleted);
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.FaClass).IsRequired(false).HasMaxLength(100);
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BountyUserCampaing>(b =>
            {
                b.ToTable("user_campaing","bounty");

                b.HasKey(x => new { x.UserId, x.BountyCampaingId });
                b.Property(x => x.IsDeleted);
                b.Property(x=> x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");
                b.Property(x => x.TotalItemCount).IsRequired(true).HasDefaultValue(0);
                b.Property(x => x.TotalCoinEarned).IsRequired(true).HasDefaultValue(0m);

                b.HasOne(x => x.BountyCampaing)
                    .WithMany()
                    .HasForeignKey(x => x.BountyCampaingId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.User)
                    .WithMany(x => x.BountyUserCampaings)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x=> x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BountyCampaingTask>(b =>
            {
                b.ToTable("campaing_task", "bounty");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.IsDeleted);
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");
                b.Property(x => x.Description).IsRequired(true).HasColumnType("text");
                b.Property(x => x.IsPrivate);


                b.HasOne(x=> x.BountyCampaingActivity)
                    .WithMany()
                    .HasForeignKey(c => c.BountyCampaingActivityId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.BountyCampaing)
                    .WithMany()
                    .HasForeignKey(x => x.BountyCampaingId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<BountyCampaingActivity>(b =>
            {
                b.ToTable("campaing_activity", "bounty");

                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
                b.Property(x => x.Price);
                b.Property(x => x.NeedToApprove);
                b.Property(x => x.IsDeleted);
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");

                b.HasOne(x => x.BountyCampaing)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.BountyCampaingId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BountyCampaingAcceptedTask>(b =>
            {
                b.ToTable("campaing_accepted_task", "bounty");

                b.HasKey(x => new { x.AcceptedByUserId, x.BountyCampaingTaskId });
                b.Property(x => x.Status);
                b.Property(x => x.Url).IsRequired(true).HasMaxLength(256);
                b.Property(x => x.Comment).IsRequired(false).HasMaxLength(256); ;
                b.Property(x => x.BlobOid).IsRequired(false);
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");

                b.HasOne(x => x.BountyCampaingTask)
                    .WithMany()
                    .HasForeignKey(x => x.BountyCampaingTaskId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.AcceptedByUser)
                    .WithMany(x => x.BountyCampaingAcceptedTasks)
                    .HasForeignKey(x => x.AcceptedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BountyCampaingTaskAssignment>(b =>
            {
                b.ToTable("campaing_task_assignment", "bounty");

                b.HasKey(x => new { x.AssignedToUserId, x.BountyCampaingTaskId });
                b.Property(x => x.IsDeleted);
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");

                b.HasOne(x => x.BountyCampaingTask)
                    .WithMany(x => x.BountyCampaingTaskAssignments)
                    .HasForeignKey(x => x.BountyCampaingTaskId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.AssignedToUser)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedToUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BountyFavoriteUser>(b =>
            {
                b.ToTable("favorite_user", "bounty");

                b.HasKey(x => new { x.UserId, x.FavoriteUserId });
                b.Property(x => x.DateCreated).IsRequired(true).HasDefaultValueSql("now()");

                b.HasOne(x => x.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.User)
                    .WithMany(x => x.BountyFavoriteUsers)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.FavoriteUser)
                    .WithMany()
                    .HasForeignKey(x => x.FavoriteUserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                //entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

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

        public DbSet<BountyCampaing> BountyCampaings { get; set; }
        public DbSet<BountyUserCampaing> BountyUserCampaings { get; set; }
        public DbSet<BountyCampaingActivity> BountyCampaingsActivity { get; set; }
        public DbSet<BountyCampaingTask> BountyCampaingTasks { get; set; }
        public DbSet<BountyCampaingAcceptedTask> BountyCampaingAcceptedTasks { get; set; }
        public DbSet<BountyCampaingTaskAssignment> BountyCampaingTaskAssignments { get; set; }
        public DbSet<BountyFavoriteUser> BountyFavoriteUsers { get; set; }
    }
}
