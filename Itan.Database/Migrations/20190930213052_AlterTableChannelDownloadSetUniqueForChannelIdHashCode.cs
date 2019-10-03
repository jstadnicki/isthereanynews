using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AlterTableChannelDownloadSetUniqueForChannelIdHashCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId_HashCode",
                table: "ChannelDownloads",
                columns: new[] { "ChannelId", "HashCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId_HashCode",
                table: "ChannelDownloads");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads",
                column: "ChannelId");
        }
    }
}
