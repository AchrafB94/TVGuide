using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVGuide.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Categories_CategoryId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Packages_PackageId",
                table: "Channels");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Categories_CategoryId",
                table: "Channels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Packages_PackageId",
                table: "Channels",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Categories_CategoryId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Packages_PackageId",
                table: "Channels");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Categories_CategoryId",
                table: "Channels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Packages_PackageId",
                table: "Channels",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }
    }
}
