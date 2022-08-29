using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVGuide.Migrations
{
    public partial class packageLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Packages",
                type: "TEXT",
                nullable: true);


        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Packages");
        }
    }
}
