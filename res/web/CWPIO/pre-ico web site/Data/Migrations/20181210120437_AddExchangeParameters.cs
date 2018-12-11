using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddExchangeParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exchange_parameters",
                schema: "exchange",
                columns: table => new
                {
                    exchanger = table.Column<string>(nullable: false),
                    eth_amount = table.Column<string>(nullable: true),
                    rate = table.Column<int>(nullable: false),
                    token_count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exchange_parameters", x => x.exchanger);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exchange_parameters",
                schema: "exchange");
        }
    }
}
