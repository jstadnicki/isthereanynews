using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class ChannelNewsRead_ReadType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReadType",
                table: "ChannelNewsReads",
                nullable: false,
                defaultValue: "Read");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadType",
                table: "ChannelNewsReads");
        }
    }
}
