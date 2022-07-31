using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVGuide.Migrations
{
    public partial class categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "Channels");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Channels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Channels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Movies" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Series" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Sports" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Documentary" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "News" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Kids" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Music" });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "OSN" });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Canal" });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "beIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CategoryId",
                table: "Channels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_PackageId",
                table: "Channels",
                column: "PackageId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Categories_CategoryId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Packages_PackageId",
                table: "Channels");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Channels_CategoryId",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_PackageId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Channels");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "Channels",
                type: "TEXT",
                nullable: true);
        }
    }
}
