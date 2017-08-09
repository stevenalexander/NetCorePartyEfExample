using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PartyData.Migrations
{
    public partial class AddCustomServiceregistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomServiceId",
                table: "Party",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomService",
                columns: table => new
                {
                    CustomServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomService", x => x.CustomServiceId);
                });

            migrationBuilder.CreateTable(
                name: "PartyCustomServiceRegistration",
                columns: table => new
                {
                    PartyCustomServiceRegistrationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomServiceId = table.Column<int>(nullable: false),
                    PartyId = table.Column<int>(nullable: false),
                    RegistrationStatus = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyCustomServiceRegistration", x => x.PartyCustomServiceRegistrationId);
                    table.ForeignKey(
                        name: "FK_PartyCustomServiceRegistration_CustomService_CustomServiceId",
                        column: x => x.CustomServiceId,
                        principalTable: "CustomService",
                        principalColumn: "CustomServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyCustomServiceRegistration_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Party_CustomServiceId",
                table: "Party",
                column: "CustomServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCustomServiceRegistration_CustomServiceId",
                table: "PartyCustomServiceRegistration",
                column: "CustomServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCustomServiceRegistration_PartyId",
                table: "PartyCustomServiceRegistration",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Party_CustomService_CustomServiceId",
                table: "Party",
                column: "CustomServiceId",
                principalTable: "CustomService",
                principalColumn: "CustomServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Party_CustomService_CustomServiceId",
                table: "Party");

            migrationBuilder.DropTable(
                name: "PartyCustomServiceRegistration");

            migrationBuilder.DropTable(
                name: "CustomService");

            migrationBuilder.DropIndex(
                name: "IX_Party_CustomServiceId",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "CustomServiceId",
                table: "Party");
        }
    }
}
