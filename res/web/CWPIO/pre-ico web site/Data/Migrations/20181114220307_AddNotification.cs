using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_protection_keys",
                schema: "core");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    text = table.Column<string>(maxLength: 250, nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    date_readed = table.Column<DateTime>(nullable: true),
                    to_user_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_users_to_user_id",
                        column: x => x.to_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_to_user_id",
                table: "notifications",
                column: "to_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");

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
    }
}
