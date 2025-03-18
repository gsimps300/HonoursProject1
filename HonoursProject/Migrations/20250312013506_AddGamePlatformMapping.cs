using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HonoursProject.Migrations
{
    /// <inheritdoc />
    public partial class AddGamePlatformMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbys_Games_GameId",
                table: "Lobbys");

            migrationBuilder.DropForeignKey(
                name: "FK_Lobbys_Users_UserId",
                table: "Lobbys");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbys_LobbyId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lobbys",
                table: "Lobbys");

            migrationBuilder.RenameTable(
                name: "Lobbys",
                newName: "Lobbies");

            migrationBuilder.RenameIndex(
                name: "IX_Lobbys_UserId",
                table: "Lobbies",
                newName: "IX_Lobbies_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lobbys_GameId",
                table: "Lobbies",
                newName: "IX_Lobbies_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lobbies",
                table: "Lobbies",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Games_GameId",
                table: "Lobbies",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Users_UserId",
                table: "Lobbies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "LobbyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Games_GameId",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Users_UserId",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lobbies",
                table: "Lobbies");

            migrationBuilder.RenameTable(
                name: "Lobbies",
                newName: "Lobbys");

            migrationBuilder.RenameIndex(
                name: "IX_Lobbies_UserId",
                table: "Lobbys",
                newName: "IX_Lobbys_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lobbies_GameId",
                table: "Lobbys",
                newName: "IX_Lobbys_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lobbys",
                table: "Lobbys",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbys_Games_GameId",
                table: "Lobbys",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbys_Users_UserId",
                table: "Lobbys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbys_LobbyId",
                table: "Users",
                column: "LobbyId",
                principalTable: "Lobbys",
                principalColumn: "LobbyId");
        }
    }
}
