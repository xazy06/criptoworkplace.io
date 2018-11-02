using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class RemoveContractAndAddAddrToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contract_addr",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.AddColumn<string>(
                name: "exchanger_contract",
                schema: "identity",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchanger_contract",
                schema: "identity",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "contract_addr",
                schema: "exchange",
                table: "exchange_status",
                nullable: true);
        }
    }
}
