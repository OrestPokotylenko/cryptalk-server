using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptalk.Migrations
{
    /// <inheritdoc />
    public partial class AddChaCha20Poly1305Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "Messages",
                newName: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "Aad",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aad",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Messages",
                newName: "Signature");
        }
    }
}
