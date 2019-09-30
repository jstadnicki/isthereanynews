using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddChannelDownloads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelDownloads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelDownloads_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDownloads_ChannelId",
                table: "ChannelDownloads",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelDownloads");
        }
    }
}
