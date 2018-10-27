using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddTotalGasCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchanger_contract",
                schema: "identity",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "total_gas_count",
                schema: "exchange",
                table: "exchange_status",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_gas_count",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.AddColumn<string>(
                name: "exchanger_contract",
                schema: "identity",
                table: "users",
                nullable: true);
        }
    }
}
