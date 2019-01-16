using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddBlockNumToExchangeParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "from_block",
                schema: "exchange",
                table: "exchange_parameters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "from_block",
                schema: "exchange",
                table: "exchange_parameters");
        }
    }
}
