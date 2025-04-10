using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Try1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetainedLicenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetainedLicenses",
                columns: table => new
                {
                    DetainedLicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseId = table.Column<int>(type: "int", nullable: false),
                    ReleaseApplicationId = table.Column<int>(type: "int", nullable: false),
                    DetainedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FineFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetainedLicenses", x => x.DetainedLicenseId);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Applications_ReleaseApplicationId",
                        column: x => x.ReleaseApplicationId,
                        principalTable: "Applications",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "LicenseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_LicenseId",
                table: "DetainedLicenses",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_ReleaseApplicationId",
                table: "DetainedLicenses",
                column: "ReleaseApplicationId",
                unique: true);
        }
    }
}
