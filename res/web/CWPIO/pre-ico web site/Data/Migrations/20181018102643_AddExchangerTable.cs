using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class AddExchangerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exchange");

            migrationBuilder.CreateTable(
                name: "exchange_status",
                schema: "exchange",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    start_tx = table.Column<string>(maxLength: 70, nullable: false),
                    current_tx = table.Column<string>(maxLength: 70, nullable: true),
                    is_ended = table.Column<bool>(nullable: false, defaultValue: false),
                    is_failed = table.Column<bool>(nullable: false, defaultValue: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exchange_status", x => x.id);
                    table.ForeignKey(
                        name: "fk_exchange_status_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_exchange_status_created_by_user_id",
                schema: "exchange",
                table: "exchange_status",
                column: "created_by_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exchange_status",
                schema: "exchange");
        }
    }
}
