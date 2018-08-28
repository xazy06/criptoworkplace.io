﻿// <auto-generated />
using CWPIO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CWPIO.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("CWPIO.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("email_confirmed");

                    b.Property<byte[]>("EthAddress")
                        .HasColumnName("eth_address")
                        .HasMaxLength(40);

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsDemo")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("is_demo")
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnName("normalized_email")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnName("normalized_user_name")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasColumnName("user_name")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasName("email_index");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("user_name_index");

                    b.ToTable("users","identity");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaing", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("FaClass")
                        .HasColumnName("fa_class")
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("pk_campaing");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_campaing_created_by_user_id");

                    b.ToTable("campaing","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingAcceptedTask", b =>
                {
                    b.Property<string>("AcceptedByUserId")
                        .HasColumnName("accepted_by_user_id");

                    b.Property<string>("BountyCampaingTaskId")
                        .HasColumnName("bounty_campaing_task_id");

                    b.Property<int?>("BlobOid")
                        .HasColumnName("blob_oid");

                    b.Property<string>("Comment")
                        .HasColumnName("comment")
                        .HasMaxLength(256);

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnName("url")
                        .HasMaxLength(256);

                    b.HasKey("AcceptedByUserId", "BountyCampaingTaskId")
                        .HasName("pk_campaing_accepted_task");

                    b.HasIndex("BountyCampaingTaskId")
                        .HasName("ix_campaing_accepted_task_bounty_campaing_task_id");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_campaing_accepted_task_created_by_user_id");

                    b.ToTable("campaing_accepted_task","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingActivity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("BountyCampaingId")
                        .IsRequired()
                        .HasColumnName("bounty_campaing_id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(200);

                    b.Property<bool>("NeedToApprove")
                        .HasColumnName("need_to_approve");

                    b.Property<decimal>("Price")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pk_campaing_activity");

                    b.HasIndex("BountyCampaingId")
                        .HasName("ix_campaing_activity_bounty_campaing_id");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_campaing_activity_created_by_user_id");

                    b.ToTable("campaing_activity","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingTask", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("BountyCampaingActivityId")
                        .IsRequired()
                        .HasColumnName("bounty_campaing_activity_id");

                    b.Property<string>("BountyCampaingId")
                        .IsRequired()
                        .HasColumnName("bounty_campaing_id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsPrivate")
                        .HasColumnName("is_private");

                    b.HasKey("Id")
                        .HasName("pk_campaing_task");

                    b.HasIndex("BountyCampaingActivityId")
                        .HasName("ix_campaing_task_bounty_campaing_activity_id");

                    b.HasIndex("BountyCampaingId")
                        .HasName("ix_campaing_task_bounty_campaing_id");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_campaing_task_created_by_user_id");

                    b.ToTable("campaing_task","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingTaskAssignment", b =>
                {
                    b.Property<string>("AssignedToUserId")
                        .HasColumnName("assigned_to_user_id");

                    b.Property<string>("BountyCampaingTaskId")
                        .HasColumnName("bounty_campaing_task_id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.HasKey("AssignedToUserId", "BountyCampaingTaskId")
                        .HasName("pk_campaing_task_assignment");

                    b.HasIndex("BountyCampaingTaskId")
                        .HasName("ix_campaing_task_assignment_bounty_campaing_task_id");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_campaing_task_assignment_created_by_user_id");

                    b.ToTable("campaing_task_assignment","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyFavoriteUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.Property<string>("FavoriteUserId")
                        .HasColumnName("favorite_user_id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.HasKey("UserId", "FavoriteUserId")
                        .HasName("pk_favorite_user");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_favorite_user_created_by_user_id");

                    b.HasIndex("FavoriteUserId")
                        .HasName("ix_favorite_user_favorite_user_id");

                    b.ToTable("favorite_user","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.BountyUserCampaing", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.Property<string>("BountyCampaingId")
                        .HasColumnName("bounty_campaing_id");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<decimal>("TotalCoinEarned")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("total_coin_earned")
                        .HasDefaultValue(0m);

                    b.Property<int>("TotalItemCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("total_item_count")
                        .HasDefaultValue(0);

                    b.HasKey("UserId", "BountyCampaingId")
                        .HasName("pk_user_campaing");

                    b.HasIndex("BountyCampaingId")
                        .HasName("ix_user_campaing_bounty_campaing_id");

                    b.HasIndex("CreatedByUserId")
                        .HasName("ix_user_campaing_created_by_user_id");

                    b.ToTable("user_campaing","bounty");
                });

            modelBuilder.Entity("CWPIO.Data.DataProtectionKey", b =>
                {
                    b.Property<string>("FriendlyName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("friendly_name")
                        .HasColumnType("text");

                    b.Property<string>("XmlData")
                        .HasColumnName("xml_data")
                        .HasColumnType("text");

                    b.HasKey("FriendlyName")
                        .HasName("pk_data_protection_keys");

                    b.ToTable("data_protection_keys","core");
                });

            modelBuilder.Entity("CWPIO.Data.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("culture")
                        .HasDefaultValue("");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnName("date_created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasMaxLength(100);

                    b.Property<bool>("EmailSend")
                        .HasColumnName("email_send");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(100);

                    b.Property<bool>("Unsubscribe")
                        .HasColumnName("unsubscribe");

                    b.HasKey("Id")
                        .HasName("pk_subscribers");

                    b.ToTable("subscribers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnName("normalized_name")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("role_name_index");

                    b.ToTable("roles","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasName("ix_role_claims_role_id");

                    b.ToTable("role_claims","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasName("ix_user_claims_user_id");

                    b.ToTable("user_claims","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnName("provider_display_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasName("ix_user_logins_user_id");

                    b.ToTable("user_logins","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.Property<string>("RoleId")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasName("ix_user_roles_role_id");

                    b.ToTable("user_roles","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("user_tokens","identity");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaing", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_campaing_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingAcceptedTask", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser", "AcceptedByUser")
                        .WithMany("BountyCampaingAcceptedTasks")
                        .HasForeignKey("AcceptedByUserId")
                        .HasConstraintName("fk_campaing_accepted_task_users_accepted_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.BountyCampaingTask", "BountyCampaingTask")
                        .WithMany()
                        .HasForeignKey("BountyCampaingTaskId")
                        .HasConstraintName("fk_campaing_accepted_task_campaing_task_bounty_campaing_task_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_campaing_accepted_task_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingActivity", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaing", "BountyCampaing")
                        .WithMany("Activities")
                        .HasForeignKey("BountyCampaingId")
                        .HasConstraintName("fk_campaing_activity_campaing_bounty_campaing_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_campaing_activity_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingTask", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaingActivity", "BountyCampaingActivity")
                        .WithMany()
                        .HasForeignKey("BountyCampaingActivityId")
                        .HasConstraintName("fk_campaing_task_campaing_activity_bounty_campaing_activity_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.BountyCampaing", "BountyCampaing")
                        .WithMany()
                        .HasForeignKey("BountyCampaingId")
                        .HasConstraintName("fk_campaing_task_campaing_bounty_campaing_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_campaing_task_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingTaskAssignment", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser", "AssignedToUser")
                        .WithMany()
                        .HasForeignKey("AssignedToUserId")
                        .HasConstraintName("fk_campaing_task_assignment_users_assigned_to_user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.BountyCampaingTask", "BountyCampaingTask")
                        .WithMany("BountyCampaingTaskAssignments")
                        .HasForeignKey("BountyCampaingTaskId")
                        .HasConstraintName("fk_campaing_task_assignment_campaing_task_bounty_campaing_task_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_campaing_task_assignment_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyFavoriteUser", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_favorite_user_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "FavoriteUser")
                        .WithMany()
                        .HasForeignKey("FavoriteUserId")
                        .HasConstraintName("fk_favorite_user_users_favorite_user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "User")
                        .WithMany("BountyFavoriteUsers")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_favorite_user_users_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.BountyUserCampaing", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaing", "BountyCampaing")
                        .WithMany()
                        .HasForeignKey("BountyCampaingId")
                        .HasConstraintName("fk_user_campaing_campaing_bounty_campaing_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .HasConstraintName("fk_user_campaing_users_created_by_user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "User")
                        .WithMany("BountyUserCampaings")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_campaing_users_user_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_role_claims_roles_role_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_claims_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_logins_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_user_roles_roles_role_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_roles_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_tokens_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
