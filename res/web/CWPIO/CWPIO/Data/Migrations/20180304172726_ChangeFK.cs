using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class ChangeFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "UserBountyCampaingItem",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId",
                table: "BountyCampaingItemType",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBountyCampaingItem_ItemType",
                table: "UserBountyCampaingItem",
                column: "ItemType");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType",
                table: "UserBountyCampaingItem",
                column: "ItemType",
                principalTable: "BountyCampaingItemType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropIndex(
                name: "IX_UserBountyCampaingItem_ItemType",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId",
                table: "BountyCampaingItemType");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "UserBountyCampaingItem",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
