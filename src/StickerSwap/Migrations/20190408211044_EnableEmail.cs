using Microsoft.EntityFrameworkCore.Migrations;

namespace StickerSwap.Migrations
{
    public partial class EnableEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableEmail",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableEmail",
                table: "AspNetUsers");
        }
    }
}
