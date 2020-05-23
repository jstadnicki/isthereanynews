using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddRestrictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelsPersons_Channels_ChannelId",
                table: "ChannelsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelsPersons_Persons_PersonId",
                table: "ChannelsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Channels_ChannelId",
                table: "News");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelsPersons_Channels_ChannelId",
                table: "ChannelsPersons",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelsPersons_Persons_PersonId",
                table: "ChannelsPersons",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Channels_ChannelId",
                table: "News",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelsPersons_Channels_ChannelId",
                table: "ChannelsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelsPersons_Persons_PersonId",
                table: "ChannelsPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Channels_ChannelId",
                table: "News");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelsPersons_Channels_ChannelId",
                table: "ChannelsPersons",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelsPersons_Persons_PersonId",
                table: "ChannelsPersons",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Channels_ChannelId",
                table: "News",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
