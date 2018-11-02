using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class RewriteChangerLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "temp_address",
                schema: "identity",
                table: "users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "addresses",
                schema: "exchange",
                columns: table => new
                {
                    address = table.Column<string>(nullable: false),
                    exchanger = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.address);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses",
                schema: "exchange");

            migrationBuilder.DropColumn(
                name: "temp_address",
                schema: "identity",
                table: "users");
        }
    }
}
