using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_Starbucks.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address1", "Address2", "Birthdate", "City", "DeletedAt", "Dk", "Email", "Name", "Phone", "RegisteredAt", "RoleId", "Salt", "State", "Surname" },
                values: new object[,]
                {
                    { new Guid("869b525f-b7e7-4a00-9313-10f3348842ec"), null, null, null, null, null, "87D88B086F61949A", "admin@change.me", "Administrator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "10F3348842EC", null, "Administrator" },
                    { new Guid("c588eff8-359a-4c25-949e-d7235525f88f"), null, null, null, null, null, "2ABEC6169453E51B", "user@example.com", "UserName", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "D7235525F88F", null, "UserSurname" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("869b525f-b7e7-4a00-9313-10f3348842ec"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c588eff8-359a-4c25-949e-d7235525f88f"));
        }
    }
}
