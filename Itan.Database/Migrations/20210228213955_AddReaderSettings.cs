using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddReaderSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReaderSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    ShowUpdatedNews = table.Column<string>(maxLength: 25, nullable: true, defaultValue: "Show"),
                    SquashNewsUpdates = table.Column<string>(maxLength: 25, nullable: true, defaultValue: "Show")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReaderSettings_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReaderSettings_PersonId",
                table: "ReaderSettings",
                column: "PersonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReaderSettings");
        }
    }
}
