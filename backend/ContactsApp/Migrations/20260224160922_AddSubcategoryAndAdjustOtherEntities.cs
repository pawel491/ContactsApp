using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoryAndAdjustOtherEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomSubcategory",
                table: "Contacts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Contacts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CustomSubcategory", "SubcategoryId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CustomSubcategory", "SubcategoryId" },
                values: new object[] { null, null });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "szef" },
                    { 2, 1, "klient" },
                    { 3, 1, "współpracownik" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SubcategoryId",
                table: "Contacts",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryId",
                table: "Contacts",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_SubcategoryId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CustomSubcategory",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Contacts");
        }
    }
}
