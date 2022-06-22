using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVGuide.Migrations
{
    public partial class removexml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Channels_Position",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_XML",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "XML",
                table: "Channels");

            migrationBuilder.AlterColumn<string>(
                name: "IdXML",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Package",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdXML",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "XML",
                table: "Channels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Position",
                table: "Channels",
                column: "Position",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_XML",
                table: "Channels",
                column: "XML");
        }
    }
}
