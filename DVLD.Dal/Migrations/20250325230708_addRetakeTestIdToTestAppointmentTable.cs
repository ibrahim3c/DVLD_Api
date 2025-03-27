using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class addRetakeTestIdToTestAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RetakeTestAppId",
                table: "Appointments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetakeTestAppId",
                table: "Appointments");
        }
    }
}
