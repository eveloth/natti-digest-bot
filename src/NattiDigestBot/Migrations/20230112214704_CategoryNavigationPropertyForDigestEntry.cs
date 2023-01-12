using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NattiDigestBot.Migrations
{
    /// <inheritdoc />
    public partial class CategoryNavigationPropertyForDigestEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DigestEntries_CategoryId",
                table: "DigestEntries",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DigestEntries_Categories_CategoryId",
                table: "DigestEntries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DigestEntries_Categories_CategoryId",
                table: "DigestEntries");

            migrationBuilder.DropIndex(
                name: "IX_DigestEntries_CategoryId",
                table: "DigestEntries");
        }
    }
}
