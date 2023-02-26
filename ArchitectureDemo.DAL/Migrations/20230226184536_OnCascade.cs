using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchitectureDemo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class OnCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_files_users_user_id",
                table: "user_files");

            migrationBuilder.AddForeignKey(
                name: "fk_user_files_users_user_id",
                table: "user_files",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_files_users_user_id",
                table: "user_files");

            migrationBuilder.AddForeignKey(
                name: "fk_user_files_users_user_id",
                table: "user_files",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
