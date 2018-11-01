using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddContractToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refund_tx",
                schema: "exchange",
                table: "exchange_status");

            migrationBuilder.RenameColumn(
                name: "token_amount",
                schema: "exchange",
                table: "exchange_status",
                newName: "contract_addr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "contract_addr",
                schema: "exchange",
                table: "exchange_status",
                newName: "token_amount");

            migrationBuilder.AddColumn<string>(
                name: "refund_tx",
                schema: "exchange",
                table: "exchange_status",
                nullable: true);
        }
    }
}
