using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddChannelNewsRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelNewsReads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    NewsId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelNewsReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelNewsReads_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChannelNewsReads_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChannelNewsReads_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelNewsReads_NewsId",
                table: "ChannelNewsReads",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelNewsReads_PersonId",
                table: "ChannelNewsReads",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelNewsReads_ChannelId_NewsId_PersonId",
                table: "ChannelNewsReads",
                columns: new[] { "ChannelId", "NewsId", "PersonId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelNewsReads");
        }
    }
}
