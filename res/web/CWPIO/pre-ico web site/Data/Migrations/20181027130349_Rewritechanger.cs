using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class Rewritechanger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "current_step",
                schema: "exchange",
                table: "exchange_status",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rate",
                schema: "exchange",
                table: "exchange_status",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "token_count",
                schema: "exchange",
                table: "exchange_status",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_step",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.DropColumn(
                name: "rate",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.DropColumn(
                name: "token_count",
                schema: "exchange",
                table: "exchange_status");
        }
    }
}
