using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Migrations
{
    public partial class AddDataProtectionKeysToNewContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "data_protection_keys",
                schema: "core",
                columns: table => new
                {
                    friendly_name = table.Column<string>(type: "text", nullable: false),
                    xml_data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_data_protection_keys", x => x.friendly_name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_protection_keys",
                schema: "core");
        }
    }
}
