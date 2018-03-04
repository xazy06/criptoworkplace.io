using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class AddBountyProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EmailSend",
                table: "Subscribers",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FaClass",
                table: "BountyCampaing",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "BountyCampaing",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaClass",
                table: "BountyCampaing");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "BountyCampaing");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailSend",
                table: "Subscribers",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }
    }
}
