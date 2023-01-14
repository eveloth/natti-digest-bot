using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NattiDigestBot.Migrations
{
    /// <inheritdoc />
    public partial class ChangePKForDigestEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DigestEntries",
                table: "DigestEntries");

            migrationBuilder.AlterColumn<int>(
                name: "DigestEntryId",
                table: "DigestEntries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DigestEntries",
                table: "DigestEntries",
                column: "DigestEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DigestEntries_DigestId_Date",
                table: "DigestEntries",
                columns: new[] { "DigestId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DigestEntries",
                table: "DigestEntries");

            migrationBuilder.DropIndex(
                name: "IX_DigestEntries_DigestId_Date",
                table: "DigestEntries");

            migrationBuilder.AlterColumn<int>(
                name: "DigestEntryId",
                table: "DigestEntries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DigestEntries",
                table: "DigestEntries",
                columns: new[] { "DigestId", "Date" });
        }
    }
}
