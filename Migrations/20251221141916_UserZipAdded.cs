using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_Starbucks.Migrations
{
    /// <inheritdoc />
    public partial class UserZipAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("869b525f-b7e7-4a00-9313-10f3348842ec"),
                column: "Zip",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c588eff8-359a-4c25-949e-d7235525f88f"),
                column: "Zip",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Users");
        }
    }
}
