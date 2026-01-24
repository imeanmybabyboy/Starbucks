using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_Starbucks.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("07e75e04-8ef2-479c-be21-6db01b5e5781"), "Drinks", "drinks" },
                    { new Guid("3d8682f6-942b-4d90-94e6-4d0f01535516"), "Fan Favorites", "fan-favorites" },
                    { new Guid("7e7becf1-c635-41b7-b4a9-5aa9cbdaca90"), "At Home Coffee", "at-home-coffee" },
                    { new Guid("b936efe2-4473-4e54-9ef5-170fb63f992b"), "Food", "food" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("07e75e04-8ef2-479c-be21-6db01b5e5781"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("3d8682f6-942b-4d90-94e6-4d0f01535516"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("7e7becf1-c635-41b7-b4a9-5aa9cbdaca90"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("b936efe2-4473-4e54-9ef5-170fb63f992b"));
        }
    }
}
