using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class AddDemoUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eth_address",
                schema: "identity",
                table: "users");

            migrationBuilder.AddColumn<byte[]>(
                name: "eth_address",
                schema: "identity",
                table: "users",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_demo",
                schema: "identity",
                table: "users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_demo",
                schema: "identity",
                table: "users");

            migrationBuilder.DropColumn(
                name: "eth_address",
                schema: "identity",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "eth_address",
                schema: "identity",
                table: "users",
                maxLength: 42,
                nullable: true);
        }
    }
}
