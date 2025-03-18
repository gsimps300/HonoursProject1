using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HonoursProject.Migrations
{
    /// <inheritdoc />
    public partial class AddLobbyParticipantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LobbyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "Users");

            migrationBuilder.AlterColumn<bool>(
                name: "MicRequired",
                table: "Lobbies",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lobbies",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LobbyParticipants",
                columns: table => new
                {
                    LobbyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyParticipants", x => new { x.LobbyId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LobbyParticipants_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "LobbyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyParticipants_UserId",
                table: "LobbyParticipants",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LobbyParticipants");

            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "MicRequired",
                table: "Lobbies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lobbies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LobbyId",
                table: "Users",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "LobbyId");
        }
    }
}
