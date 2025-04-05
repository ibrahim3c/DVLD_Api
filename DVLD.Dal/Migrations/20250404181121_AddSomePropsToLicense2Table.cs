using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddSomePropsToLicense2Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsReplaced",
                table: "Licenses",
                newName: "IsLost");

            migrationBuilder.AddColumn<bool>(
                name: "IsDamaged",
                table: "Licenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDamaged",
                table: "Licenses");

            migrationBuilder.RenameColumn(
                name: "IsLost",
                table: "Licenses",
                newName: "IsReplaced");
        }
    }
}
