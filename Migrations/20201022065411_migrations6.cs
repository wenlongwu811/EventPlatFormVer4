﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace EventPlatFormVer4.Migrations
{
    public partial class migrations6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sponsors_SponsorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Participatants_Events_EventId",
                table: "Participatants");

            migrationBuilder.DropIndex(
                name: "IX_Participatants_EventId",
                table: "Participatants");

            migrationBuilder.DropIndex(
                name: "IX_Events_SponsorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Participatants");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "Participatants",
                type: "string",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SponsorId",
                table: "Events",
                type: "string",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participatants_EventId",
                table: "Participatants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SponsorId",
                table: "Events",
                column: "SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sponsors_SponsorId",
                table: "Events",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participatants_Events_EventId",
                table: "Participatants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
