using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class addTestTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TestTypes",
                columns: new[] { "Id", "Description", "Title", "TypeFee" },
                values: new object[,]
                {
                    { 1, "This assesses the applicant's visual acuity to ensure they meet the minimum vision standards.", "Vision Test", 10.00000m },
                    { 2, "This test assesses the applicant's knowledge of traffic laws, road signs, and safe driving practices.", "Written (Theory) Test", 20.00000m },
                    { 3, "This test evaluates the applicant's driving skills and ability to operate a vehicle safely.", "Practical (Street) Test", 35.00000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestTypes");
        }
    }
}
