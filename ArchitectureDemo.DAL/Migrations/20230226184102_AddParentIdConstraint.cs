using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchitectureDemo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIdConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_users_parent_id",
                table: "users");

            migrationBuilder.RenameIndex(
                name: "user_email_key",
                table: "users",
                newName: "users_email_key");

            migrationBuilder.AddForeignKey(
                name: "users_parent_id_fkey",
                table: "users",
                column: "parent_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "users_parent_id_fkey",
                table: "users");

            migrationBuilder.RenameIndex(
                name: "users_email_key",
                table: "users",
                newName: "user_email_key");

            migrationBuilder.AddForeignKey(
                name: "fk_users_users_parent_id",
                table: "users",
                column: "parent_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
