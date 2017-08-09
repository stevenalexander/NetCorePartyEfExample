using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartyData.Migrations
{
    public partial class AddDataCustomServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomService]([Name]) VALUES ('Customer service')");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomService]([Name]) VALUES ('Payment service')");
            migrationBuilder.Sql("INSERT INTO [dbo].[CustomService]([Name]) VALUES ('Address lookup service')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[CustomService]");
        }
    }
}
