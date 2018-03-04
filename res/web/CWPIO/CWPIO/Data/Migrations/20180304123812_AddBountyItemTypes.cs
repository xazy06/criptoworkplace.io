using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class AddBountyItemTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBlocked",
                table: "BountyCampaing",
                newName: "IsDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "FaClass",
                table: "BountyCampaing",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BountyCampaingItemType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BountyCampaingId = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    NeedToApprove = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BountyCampaingItemType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BountyCampaingItemType_BountyCampaing_BountyCampaingId",
                        column: x => x.BountyCampaingId,
                        principalTable: "BountyCampaing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BountyCampaingItemType_BountyCampaingId",
                table: "BountyCampaingItemType",
                column: "BountyCampaingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BountyCampaingItemType");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "BountyCampaing",
                newName: "IsBlocked");

            migrationBuilder.AlterColumn<string>(
                name: "FaClass",
                table: "BountyCampaing",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
