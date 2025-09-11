using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptalk.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class IndexMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId_Timestamp_Id",
                table: "Messages",
                columns: new[] { "ChatId", "Timestamp", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId_Timestamp_Id",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");
        }
    }
}
