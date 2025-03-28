﻿using Microsoft.EntityFrameworkCore.Migrations;
using DVLD.Core.Constants;

#nullable disable

namespace DVLD.Dal.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRole = Roles.AdminRole;
            migrationBuilder.Sql($@"
            INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Guid.NewGuid()}', '{adminRole}', '{adminRole.ToUpper()}', '{Guid.NewGuid()}')");


            var UserRole = Roles.UserRole;
            migrationBuilder.Sql($@"
            INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Guid.NewGuid()}', '{UserRole}', '{UserRole.ToUpper()}', '{Guid.NewGuid()}')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Delete From Roles");
        }
    }
}
