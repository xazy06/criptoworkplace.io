using Microsoft.EntityFrameworkCore.Migrations;

namespace pre_ico_web_site.Data.Migrations
{
    public partial class SetEthAddrLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_campaing_task_assignment_campaing_task_bounty_campaing_task_id",
                schema: "bounty",
                table: "campaing_task_assignment");

            migrationBuilder.AddForeignKey(
                name: "fk_campaing_task_assignment_campaing_task_bounty_campaing_task~",
                schema: "bounty",
                table: "campaing_task_assignment",
                column: "bounty_campaing_task_id",
                principalSchema: "bounty",
                principalTable: "campaing_task",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_campaing_task_assignment_campaing_task_bounty_campaing_task~",
                schema: "bounty",
                table: "campaing_task_assignment");

            migrationBuilder.AddForeignKey(
                name: "fk_campaing_task_assignment_campaing_task_bounty_campaing_task_id",
                schema: "bounty",
                table: "campaing_task_assignment",
                column: "bounty_campaing_task_id",
                principalSchema: "bounty",
                principalTable: "campaing_task",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
