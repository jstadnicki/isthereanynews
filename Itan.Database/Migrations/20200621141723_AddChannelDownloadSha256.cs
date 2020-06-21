using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddChannelDownloadSha256 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SHA256",
                table: "ChannelDownloads",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SHA256",
                table: "ChannelDownloads");
        }
    }
}
