using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Itan.Database.Migrations
{
    public partial class PeronsPersons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonsPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TargetPersonId = table.Column<Guid>(nullable: false),
                    FollowerPersonId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonsPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonsPersons_Persons_FollowerPersonId",
                        column: x => x.FollowerPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonsPersons_Persons_TargetPersonId",
                        column: x => x.TargetPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonsPersons_FollowerPersonId",
                table: "PersonsPersons",
                column: "FollowerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonsPersons_TargetPersonId",
                table: "PersonsPersons",
                column: "TargetPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonsPersons_Id_FollowerPersonId_TargetPersonId",
                table: "PersonsPersons",
                columns: new[] { "Id", "FollowerPersonId", "TargetPersonId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonsPersons");
        }
    }
}
