using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_Starbucks.Migrations
{
    /// <inheritdoc />
    public partial class RealUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c588eff8-359a-4c25-949e-d7235525f88f"),
                columns: new[] { "Email", "Name", "Surname" },
                values: new object[] { "jack.daniel@example.com", "Jack", "Daniel" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c588eff8-359a-4c25-949e-d7235525f88f"),
                columns: new[] { "Email", "Name", "Surname" },
                values: new object[] { "user@example.com", "UserName", "UserSurname" });
        }
    }
}
