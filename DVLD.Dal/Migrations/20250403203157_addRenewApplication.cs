using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class addRenewApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Applications",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpiredLicenseId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ExpiredLicenseId",
                table: "Applications",
                column: "ExpiredLicenseId",
                unique: true,
                filter: "[ExpiredLicenseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Licenses_ExpiredLicenseId",
                table: "Applications",
                column: "ExpiredLicenseId",
                principalTable: "Licenses",
                principalColumn: "LicenseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Licenses_ExpiredLicenseId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ExpiredLicenseId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ExpiredLicenseId",
                table: "Applications");
        }
    }
}
