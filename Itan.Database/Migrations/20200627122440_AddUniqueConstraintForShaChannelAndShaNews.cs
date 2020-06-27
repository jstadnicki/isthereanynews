using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddUniqueConstraintForShaChannelAndShaNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_News_ChannelId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads");

            migrationBuilder.CreateIndex(
                name: "IX_News_ChannelId_SHA256",
                table: "News",
                columns: new[] { "ChannelId", "SHA256" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId_SHA256",
                table: "ChannelDownloads",
                columns: new[] { "ChannelId", "SHA256" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_News_ChannelId_SHA256",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId_SHA256",
                table: "ChannelDownloads");

            migrationBuilder.CreateIndex(
                name: "IX_News_ChannelId",
                table: "News",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads",
                column: "ChannelId");
        }
    }
}
