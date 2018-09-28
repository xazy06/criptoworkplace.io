using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddBouties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BountyCampaing",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BountyCampaing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBountyCampaing",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    BountyCampaingId = table.Column<string>(nullable: false),
                    TotalCoinEarned = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TotalItemCount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBountyCampaing", x => new { x.UserId, x.BountyCampaingId });
                    table.ForeignKey(
                        name: "FK_UserBountyCampaing_BountyCampaing_BountyCampaingId",
                        column: x => x.BountyCampaingId,
                        principalTable: "BountyCampaing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBountyCampaing_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBountyCampaing_BountyCampaingId",
                table: "UserBountyCampaing",
                column: "BountyCampaingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBountyCampaing");

            migrationBuilder.DropTable(
                name: "BountyCampaing");
        }
    }
}
