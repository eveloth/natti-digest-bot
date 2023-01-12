using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NattiDigestBot.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCategoryKeyword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_Keyword",
                table: "Categories",
                column: "Keyword",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Keyword",
                table: "Categories");
        }
    }
}
