using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class addAppTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
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
                    table.PrimaryKey("PK_ApplicationTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationTypes",
                columns: new[] { "Id", "Description", "Title", "TypeFee" },
                values: new object[,]
                {
                    { 1, "Apply for a new local driving license.", "New Local Driving License Service", 15.00000m },
                    { 2, "Renew an existing driving license.", "Renew Driving License Service", 5.00000m },
                    { 3, "Get a replacement for a lost license.", "Replacement for a Lost Driving License", 10.00000m },
                    { 4, "Replace a damaged driving license.", "Replacement for a Damaged Driving License", 5.00000m },
                    { 5, "Release a detained driving license.", "Release Detained Driving License", 15.00000m },
                    { 6, "Apply for an international driving license.", "New International Driving License", 51.00000m },
                    { 7, "Retake a driving test after failure.", "Retake Test", 5.00000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTypes");
        }
    }
}
