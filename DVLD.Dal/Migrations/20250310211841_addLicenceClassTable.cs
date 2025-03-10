using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class addLicenceClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicenseClassId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LicenseClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    ValidityPeriod = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseClasses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LicenseClasses",
                columns: new[] { "Id", "Description", "Fee", "MinAge", "Name", "ValidityPeriod" },
                values: new object[,]
                {
                    { 1, "It allows the driver to drive small motorcycles.", 15.00m, 18, "Class 1 - Small Motorcycle", 5 },
                    { 2, "Heavy Motorcycle License (Large Motorcycle).", 30.00m, 21, "Class 2 - Heavy Motorcycle License", 5 },
                    { 3, "Ordinary driving license (car licence).", 20.00m, 18, "Class 3 - Ordinary driving license", 10 },
                    { 4, "Commercial driving license (taxi/limousine).", 200.00m, 21, "Class 4 - Commercial", 10 },
                    { 5, "Agricultural and work vehicles used in farming.", 50.00m, 21, "Class 5 - Agricultural", 10 },
                    { 6, "Small and medium bus license.", 250.00m, 21, "Class 6 - Small and medium bus", 10 },
                    { 7, "Truck and heavy vehicle license.", 300.00m, 21, "Class 7 - Truck and heavy vehicle", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_LicenseClassId",
                table: "Applications",
                column: "LicenseClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_LicenseClasses_LicenseClassId",
                table: "Applications",
                column: "LicenseClassId",
                principalTable: "LicenseClasses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_LicenseClasses_LicenseClassId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "LicenseClasses");

            migrationBuilder.DropIndex(
                name: "IX_Applications_LicenseClassId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "LicenseClassId",
                table: "Applications");
        }
    }
}
