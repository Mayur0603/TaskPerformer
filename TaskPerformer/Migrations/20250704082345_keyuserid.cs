using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPerformer.Migrations
{
    /// <inheritdoc />
    public partial class keyuserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "todo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_todo_UserId",
                table: "todo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_todo_users_UserId",
                table: "todo",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todo_users_UserId",
                table: "todo");

            migrationBuilder.DropIndex(
                name: "IX_todo_UserId",
                table: "todo");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "todo");
        }
    }
}
