using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class NewsPublicationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "News",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "News");
        }
    }
}
