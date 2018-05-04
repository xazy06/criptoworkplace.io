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
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("users","identity");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaing", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FaClass")
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("BountyCampaing");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingItemType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BountyCampaingId")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("NeedToApprove");

                    b.Property<decimal>("Price")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0m);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("BountyCampaingId");

                    b.ToTable("BountyCampaingItemType");
                });

            modelBuilder.Entity("CWPIO.Data.DataProtectionKey", b =>
                {
                    b.Property<string>("FriendlyName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FriendlyName")
                        .HasColumnType("text");

                    b.Property<string>("XmlData")
                        .HasColumnName("XmlData")
                        .HasColumnType("text");

                    b.HasKey("FriendlyName");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("CWPIO.Data.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Culture")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("EmailSend");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("Unsubscribe");

                    b.HasKey("Id");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("CWPIO.Data.UserBountyCampaing", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("BountyCampaingId");

                    b.Property<decimal>("TotalCoinEarned")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0m);

                    b.Property<int>("TotalItemCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("UserId", "BountyCampaingId");

                    b.HasIndex("BountyCampaingId");

                    b.ToTable("UserBountyCampaing");
                });

            modelBuilder.Entity("CWPIO.Data.UserBountyCampaingItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BountyCampaingId")
                        .IsRequired();

                    b.Property<bool?>("IsAccepted");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ItemType");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ItemType", "BountyCampaingId");

                    b.HasIndex("UserId", "BountyCampaingId");

                    b.ToTable("UserBountyCampaingItem");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("roles","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("roleclaims","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("userclaims","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("userlogins","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("userroles","identity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("usertokens","identity");
                });

            modelBuilder.Entity("CWPIO.Data.BountyCampaingItemType", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaing", "BountyCampaing")
                        .WithMany("ItemTypes")
                        .HasForeignKey("BountyCampaingId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.UserBountyCampaing", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaing", "BountyCampaing")
                        .WithMany("UserBounties")
                        .HasForeignKey("BountyCampaingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.ApplicationUser", "User")
                        .WithMany("UserBounties")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CWPIO.Data.UserBountyCampaingItem", b =>
                {
                    b.HasOne("CWPIO.Data.BountyCampaingItemType")
                        .WithMany()
                        .HasForeignKey("ItemType", "BountyCampaingId")
                        .HasPrincipalKey("TypeId", "BountyCampaingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CWPIO.Data.UserBountyCampaing", "UserBounty")
                        .WithMany("Items")
                        .HasForeignKey("UserId", "BountyCampaingId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CWPIO.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
