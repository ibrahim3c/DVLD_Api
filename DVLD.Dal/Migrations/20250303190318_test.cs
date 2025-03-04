using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicants_Country_CountryID",
                table: "applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_applicants_Country_NationalityCountryID",
                table: "applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_applicants_ApplicantID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApplicantID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_applicants_NationalityCountryID",
                table: "applicants");

            migrationBuilder.DropColumn(
                name: "ApplicantID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NationalityCountryID",
                table: "applicants");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "Country",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "applicants",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "ApplicantID",
                table: "applicants",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_applicants_CountryID",
                table: "applicants",
                newName: "IX_applicants_CountryId");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "applicants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_applicants_Country_CountryId",
                table: "applicants",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicants_Country_CountryId",
                table: "applicants");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Country",
                newName: "CountryID");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "applicants",
                newName: "CountryID");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "applicants",
                newName: "ApplicantID");

            migrationBuilder.RenameIndex(
                name: "IX_applicants_CountryId",
                table: "applicants",
                newName: "IX_applicants_CountryID");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryID",
                table: "applicants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "NationalityCountryID",
                table: "applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApplicantID",
                table: "Users",
                column: "ApplicantID");

            migrationBuilder.CreateIndex(
                name: "IX_applicants_NationalityCountryID",
                table: "applicants",
                column: "NationalityCountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_applicants_Country_CountryID",
                table: "applicants",
                column: "CountryID",
                principalTable: "Country",
                principalColumn: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_applicants_Country_NationalityCountryID",
                table: "applicants",
                column: "NationalityCountryID",
                principalTable: "Country",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_applicants_ApplicantID",
                table: "Users",
                column: "ApplicantID",
                principalTable: "applicants",
                principalColumn: "ApplicantID");
        }
    }
}
