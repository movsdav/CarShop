using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_relation_btw_carModel_appUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "CarModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_AppUserId",
                table: "CarModels",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_AspNetUsers_AppUserId",
                table: "CarModels",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_AspNetUsers_AppUserId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_AppUserId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "CarModels");
        }
    }
}
