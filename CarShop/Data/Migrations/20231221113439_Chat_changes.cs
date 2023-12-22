using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class Chat_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatChannelId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatChannels_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatChannels_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatChannelId",
                table: "Messages",
                column: "ChatChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatChannels_User1Id_User2Id",
                table: "ChatChannels",
                columns: new[] { "User1Id", "User2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatChannels_User2Id",
                table: "ChatChannels",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages",
                column: "ChatChannelId",
                principalTable: "ChatChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatChannels");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatChannelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatChannelId",
                table: "Messages");
        }
    }
}
