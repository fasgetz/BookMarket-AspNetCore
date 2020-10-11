using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarket.Migrations.Users
{
    public partial class AddDateRegistrationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateRegistration",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateRegistration",
                table: "AspNetUsers");
        }
    }
}
