using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartyData.Migrations
{
    public partial class AddPartyIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Party",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Party_Name",
                table: "Party",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Party_Name",
                table: "Party");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Party",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
