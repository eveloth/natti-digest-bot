using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NattiDigestBot.Migrations
{
    /// <inheritdoc />
    public partial class AddSentDigestMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PinnedDigestMessageId",
                table: "Accounts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PinnedDigestMessageId",
                table: "Accounts");
        }
    }
}
