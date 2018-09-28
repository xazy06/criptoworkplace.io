using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddBountyItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBountyCampaingItem",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BountyCampaingId = table.Column<string>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 255, nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBountyCampaingItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBountyCampaingItem_UserBountyCampaing_UserId_BountyCampaingId",
                        columns: x => new { x.UserId, x.BountyCampaingId },
                        principalTable: "UserBountyCampaing",
                        principalColumns: new[] { "UserId", "BountyCampaingId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBountyCampaingItem_UserId_BountyCampaingId",
                table: "UserBountyCampaingItem",
                columns: new[] { "UserId", "BountyCampaingId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBountyCampaingItem");
        }
    }
}
