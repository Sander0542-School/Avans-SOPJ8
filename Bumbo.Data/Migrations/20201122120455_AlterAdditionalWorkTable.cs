using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bumbo.Data.Migrations
{
    public partial class AlterAdditionalWorkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "UserAdditionalWorks");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "UserAdditionalWorks",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "UserAdditionalWorks",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "UserAdditionalWorks");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "UserAdditionalWorks");

            migrationBuilder.AddColumn<double>(
                name: "Hours",
                table: "UserAdditionalWorks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
