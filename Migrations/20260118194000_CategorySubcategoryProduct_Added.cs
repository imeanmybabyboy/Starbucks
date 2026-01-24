using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_Starbucks.Migrations
{
    /// <inheritdoc />
    public partial class CategorySubcategoryProduct_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubcategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    CaloriesFromFat = table.Column<int>(type: "int", nullable: false),
                    TotalFatG = table.Column<int>(type: "int", nullable: false),
                    TotalFatPercent = table.Column<int>(type: "int", nullable: false),
                    SaturatedFatG = table.Column<int>(type: "int", nullable: false),
                    SaturatedFatPercent = table.Column<int>(type: "int", nullable: false),
                    TransFatG = table.Column<int>(type: "int", nullable: false),
                    TransFatPercent = table.Column<int>(type: "int", nullable: false),
                    CholesterolMg = table.Column<int>(type: "int", nullable: false),
                    CholesterolPercent = table.Column<int>(type: "int", nullable: false),
                    SodiumMg = table.Column<int>(type: "int", nullable: false),
                    SodiumPercent = table.Column<int>(type: "int", nullable: false),
                    TotalCarbs = table.Column<int>(type: "int", nullable: false),
                    DietFiber = table.Column<int>(type: "int", nullable: false),
                    Sugars = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<int>(type: "int", nullable: false),
                    Allergens = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Subcategory_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SizeId",
                table: "Product",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubcategoryId",
                table: "Product",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_CategoryId",
                table: "Subcategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_Slug",
                table: "Subcategory",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Subcategory");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
