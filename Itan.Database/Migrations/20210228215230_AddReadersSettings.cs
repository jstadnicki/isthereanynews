using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class AddReadersSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadersSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    ShowUpdatedNews = table.Column<string>(maxLength: 25, nullable: true, defaultValue: "Show"),
                    SquashNewsUpdates = table.Column<string>(maxLength: 25, nullable: true, defaultValue: "Show")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadersSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadersSettings_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadersSettings_PersonId",
                table: "ReadersSettings",
                column: "PersonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadersSettings");
        }
    }
}
