using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "UserBountyCampaingItem",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId_BountyCampaingId",
                table: "BountyCampaingItemType",
                columns: new[] { "TypeId", "BountyCampaingId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBountyCampaingItem_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem",
                columns: new[] { "ItemType", "BountyCampaingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem",
                columns: new[] { "ItemType", "BountyCampaingId" },
                principalTable: "BountyCampaingItemType",
                principalColumns: new[] { "TypeId", "BountyCampaingId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropIndex(
                name: "IX_UserBountyCampaingItem_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId_BountyCampaingId",
                table: "BountyCampaingItemType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "UserBountyCampaingItem",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
