using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class RemoveHashCodeSetSha256AsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_News_ChannelId_HashCode",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId_HashCode",
                table: "ChannelDownloads");

            migrationBuilder.DropColumn(
                name: "HashCode",
                table: "News");

            migrationBuilder.DropColumn(
                name: "HashCode",
                table: "ChannelDownloads");

            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "News",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "ChannelDownloads",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_ChannelId",
                table: "News",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_News_ChannelId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads");

            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "News",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AddColumn<long>(
                name: "HashCode",
                table: "News",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "ChannelDownloads",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AddColumn<int>(
                name: "HashCode",
                table: "ChannelDownloads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_News_ChannelId_HashCode",
                table: "News",
                columns: new[] { "ChannelId", "HashCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId_HashCode",
                table: "ChannelDownloads",
                columns: new[] { "ChannelId", "HashCode" },
                unique: true);
        }
    }
}
