using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPerformer.Migrations
{
    /// <inheritdoc />
    public partial class foreignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_user_Username",
                table: "users",
                newName: "IX_users_Username");

            migrationBuilder.RenameIndex(
                name: "IX_user_Email",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "todo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "todo");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameIndex(
                name: "IX_users_Username",
                table: "user",
                newName: "IX_user_Username");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                table: "user",
                newName: "IX_user_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");
        }
    }
}
