using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddAmountToExchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "eth_amount",
                schema: "exchange",
                table: "exchange_status",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "token_amount",
                schema: "exchange",
                table: "exchange_status",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eth_amount",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.DropColumn(
                name: "token_amount",
                schema: "exchange",
                table: "exchange_status");
        }
    }
}
