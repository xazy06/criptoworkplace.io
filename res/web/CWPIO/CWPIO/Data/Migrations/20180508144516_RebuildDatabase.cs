using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class RebuildDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_bounty_campaing_item");

            migrationBuilder.DropTable(
                name: "bounty_campaing_item_type");

            migrationBuilder.DropTable(
                name: "user_bounty_campaing");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bounty_campaing",
                table: "bounty_campaing");

            migrationBuilder.EnsureSchema(
                name: "bounty");

            migrationBuilder.RenameTable(
                name: "bounty_campaing",
                newName: "campaing",
                newSchema: "bounty");

            migrationBuilder.AddColumn<string>(
                name: "created_by_user_id",
                schema: "bounty",
                table: "campaing",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("update bounty.campaing set created_by_user_id = (select id from identity.users limit 1)");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "bounty",
                table: "campaing",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "pk_campaing",
                schema: "bounty",
                table: "campaing",
                column: "id");

            migrationBuilder.CreateTable(
                name: "campaing_activity",
                schema: "bounty",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    need_to_approve = table.Column<bool>(nullable: false),
                    price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaing_activity", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaing_activity_campaing_bounty_campaing_id",
                        column: x => x.bounty_campaing_id,
                        principalSchema: "bounty",
                        principalTable: "campaing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_activity_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorite_user",
                schema: "bounty",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    favorite_user_id = table.Column<string>(nullable: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favorite_user", x => new { x.user_id, x.favorite_user_id });
                    table.ForeignKey(
                        name: "fk_favorite_user_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_favorite_user_users_favorite_user_id",
                        column: x => x.favorite_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_favorite_user_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_campaing",
                schema: "bounty",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(nullable: false),
                    total_coin_earned = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    total_item_count = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_campaing", x => new { x.user_id, x.bounty_campaing_id });
                    table.ForeignKey(
                        name: "fk_user_campaing_campaing_bounty_campaing_id",
                        column: x => x.bounty_campaing_id,
                        principalSchema: "bounty",
                        principalTable: "campaing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_campaing_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_campaing_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "campaing_task",
                schema: "bounty",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    bounty_campaing_activity_id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    is_private = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaing_task", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaing_task_campaing_activity_bounty_campaing_activity_id",
                        column: x => x.bounty_campaing_activity_id,
                        principalSchema: "bounty",
                        principalTable: "campaing_activity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_task_campaing_bounty_campaing_id",
                        column: x => x.bounty_campaing_id,
                        principalSchema: "bounty",
                        principalTable: "campaing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_task_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "campaing_accepted_task",
                schema: "bounty",
                columns: table => new
                {
                    accepted_by_user_id = table.Column<string>(nullable: false),
                    bounty_campaing_task_id = table.Column<string>(nullable: false),
                    blob_oid = table.Column<int>(nullable: true),
                    comment = table.Column<string>(maxLength: 256, nullable: true),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    status = table.Column<int>(nullable: false),
                    url = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaing_accepted_task", x => new { x.accepted_by_user_id, x.bounty_campaing_task_id });
                    table.ForeignKey(
                        name: "fk_campaing_accepted_task_users_accepted_by_user_id",
                        column: x => x.accepted_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_accepted_task_campaing_task_bounty_campaing_task_id",
                        column: x => x.bounty_campaing_task_id,
                        principalSchema: "bounty",
                        principalTable: "campaing_task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_accepted_task_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "campaing_task_assignment",
                schema: "bounty",
                columns: table => new
                {
                    assigned_to_user_id = table.Column<string>(nullable: false),
                    bounty_campaing_task_id = table.Column<string>(nullable: false),
                    created_by_user_id = table.Column<string>(nullable: false),
                    date_created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaing_task_assignment", x => new { x.assigned_to_user_id, x.bounty_campaing_task_id });
                    table.ForeignKey(
                        name: "fk_campaing_task_assignment_users_assigned_to_user_id",
                        column: x => x.assigned_to_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_task_assignment_campaing_task_bounty_campaing_task_id",
                        column: x => x.bounty_campaing_task_id,
                        principalSchema: "bounty",
                        principalTable: "campaing_task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_campaing_task_assignment_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaing_created_by_user_id",
                schema: "bounty",
                table: "campaing",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_accepted_task_bounty_campaing_task_id",
                schema: "bounty",
                table: "campaing_accepted_task",
                column: "bounty_campaing_task_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_accepted_task_created_by_user_id",
                schema: "bounty",
                table: "campaing_accepted_task",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_activity_bounty_campaing_id",
                schema: "bounty",
                table: "campaing_activity",
                column: "bounty_campaing_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_activity_created_by_user_id",
                schema: "bounty",
                table: "campaing_activity",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_task_bounty_campaing_activity_id",
                schema: "bounty",
                table: "campaing_task",
                column: "bounty_campaing_activity_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_task_bounty_campaing_id",
                schema: "bounty",
                table: "campaing_task",
                column: "bounty_campaing_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_task_created_by_user_id",
                schema: "bounty",
                table: "campaing_task",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_task_assignment_bounty_campaing_task_id",
                schema: "bounty",
                table: "campaing_task_assignment",
                column: "bounty_campaing_task_id");

            migrationBuilder.CreateIndex(
                name: "ix_campaing_task_assignment_created_by_user_id",
                schema: "bounty",
                table: "campaing_task_assignment",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_favorite_user_created_by_user_id",
                schema: "bounty",
                table: "favorite_user",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_favorite_user_favorite_user_id",
                schema: "bounty",
                table: "favorite_user",
                column: "favorite_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_campaing_bounty_campaing_id",
                schema: "bounty",
                table: "user_campaing",
                column: "bounty_campaing_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_campaing_created_by_user_id",
                schema: "bounty",
                table: "user_campaing",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_campaing_users_created_by_user_id",
                schema: "bounty",
                table: "campaing",
                column: "created_by_user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_campaing_users_created_by_user_id",
                schema: "bounty",
                table: "campaing");

            migrationBuilder.DropTable(
                name: "campaing_accepted_task",
                schema: "bounty");

            migrationBuilder.DropTable(
                name: "campaing_task_assignment",
                schema: "bounty");

            migrationBuilder.DropTable(
                name: "favorite_user",
                schema: "bounty");

            migrationBuilder.DropTable(
                name: "user_campaing",
                schema: "bounty");

            migrationBuilder.DropTable(
                name: "campaing_task",
                schema: "bounty");

            migrationBuilder.DropTable(
                name: "campaing_activity",
                schema: "bounty");

            migrationBuilder.DropPrimaryKey(
                name: "pk_campaing",
                schema: "bounty",
                table: "campaing");

            migrationBuilder.DropIndex(
                name: "ix_campaing_created_by_user_id",
                schema: "bounty",
                table: "campaing");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                schema: "bounty",
                table: "campaing");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "bounty",
                table: "campaing");

            migrationBuilder.RenameTable(
                name: "campaing",
                schema: "bounty",
                newName: "bounty_campaing");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bounty_campaing",
                table: "bounty_campaing",
                column: "id");

            migrationBuilder.CreateTable(
                name: "bounty_campaing_item_type",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    need_to_approve = table.Column<bool>(nullable: false),
                    price = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    type_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bounty_campaing_item_type", x => x.id);
                    table.UniqueConstraint("ak_bounty_campaing_item_type_type_id_bounty_campaing_id", x => new { x.type_id, x.bounty_campaing_id });
                    table.ForeignKey(
                        name: "fk_bounty_campaing_item_type_bounty_campaing_bounty_campaing_id",
                        column: x => x.bounty_campaing_id,
                        principalTable: "bounty_campaing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_bounty_campaing",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    total_coin_earned = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    total_item_count = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_bounty_campaing", x => new { x.user_id, x.bounty_campaing_id });
                    table.ForeignKey(
                        name: "fk_user_bounty_campaing_bounty_campaing_bounty_campaing_id",
                        column: x => x.bounty_campaing_id,
                        principalTable: "bounty_campaing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_bounty_campaing_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_bounty_campaing_item",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    bounty_campaing_id = table.Column<string>(nullable: false),
                    is_accepted = table.Column<bool>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    item_type = table.Column<int>(nullable: false),
                    url = table.Column<string>(maxLength: 255, nullable: false),
                    user_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_bounty_campaing_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_bounty_campaing_item_bounty_campaing_item_type_item_type_bounty_campaing_id",
                        columns: x => new { x.item_type, x.bounty_campaing_id },
                        principalTable: "bounty_campaing_item_type",
                        principalColumns: new[] { "type_id", "bounty_campaing_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_bounty_campaing_item_user_bounty_campaing_user_id_bounty_campaing_id",
                        columns: x => new { x.user_id, x.bounty_campaing_id },
                        principalTable: "user_bounty_campaing",
                        principalColumns: new[] { "user_id", "bounty_campaing_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bounty_campaing_item_type_bounty_campaing_id",
                table: "bounty_campaing_item_type",
                column: "bounty_campaing_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_bounty_campaing_bounty_campaing_id",
                table: "user_bounty_campaing",
                column: "bounty_campaing_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_bounty_campaing_item_item_type_bounty_campaing_id",
                table: "user_bounty_campaing_item",
                columns: new[] { "item_type", "bounty_campaing_id" });

            migrationBuilder.CreateIndex(
                name: "ix_user_bounty_campaing_item_user_id_bounty_campaing_id",
                table: "user_bounty_campaing_item",
                columns: new[] { "user_id", "bounty_campaing_id" });
        }
    }
}
