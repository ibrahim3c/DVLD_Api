using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCreatedByFromLicenseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Users_CreatedBy",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_CreatedBy",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Licenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Licenses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_CreatedBy",
                table: "Licenses",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Users_CreatedBy",
                table: "Licenses",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
