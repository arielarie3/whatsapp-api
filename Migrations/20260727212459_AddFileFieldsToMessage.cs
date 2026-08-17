using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFileFieldsToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId1",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Messages",
                newName: "FileUrl");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "FileType");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Messages",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId1",
                table: "Messages",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId1",
                table: "Messages",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
