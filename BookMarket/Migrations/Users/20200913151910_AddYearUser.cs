using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations.Users
{
    public partial class AddYearUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "AspNetUsers");
        }
    }
}
