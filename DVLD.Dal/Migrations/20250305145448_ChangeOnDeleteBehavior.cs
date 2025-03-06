using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicants_Users_UserId",
                table: "applicants");

            migrationBuilder.AddForeignKey(
                name: "FK_applicants_Users_UserId",
                table: "applicants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicants_Users_UserId",
                table: "applicants");

            migrationBuilder.AddForeignKey(
                name: "FK_applicants_Users_UserId",
                table: "applicants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
