using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bumbo.Data.Migrations
{
    public partial class Bumbo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(maxLength: 7, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClockSystemTags",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(maxLength: 20, nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClockSystemTags", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_ClockSystemTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForecastStandard",
                columns: table => new
                {
                    Activity = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastStandard", x => x.Activity);
                });

            migrationBuilder.CreateTable(
                name: "UserAdditionalWorks",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Hours = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionalWorks", x => new { x.UserId, x.Day });
                    table.ForeignKey(
                        name: "FK_UserAdditionalWorks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAvailabilities",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvailabilities", x => new { x.UserId, x.Day });
                    table.ForeignKey(
                        name: "FK_UserAvailabilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forecast",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Department = table.Column<int>(nullable: false),
                    WorkingHours = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecast", x => new { x.BranchId, x.Date, x.Department });
                    table.ForeignKey(
                        name: "FK_Forecast_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Department = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shifts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchForecastStandard",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false),
                    Activity = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchForecastStandard", x => new { x.BranchId, x.Activity });
                    table.ForeignKey(
                        name: "FK_BranchForecastStandard_ForecastStandard_Activity",
                        column: x => x.Activity,
                        principalTable: "ForecastStandard",
                        principalColumn: "Activity",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchForecastStandard_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkedShifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(nullable: false),
                    Sick = table.Column<bool>(nullable: false, defaultValue: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkedShifts", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_WorkedShifts_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchForecastStandard_Activity",
                table: "BranchForecastStandard",
                column: "Activity");

            migrationBuilder.CreateIndex(
                name: "IX_ClockSystemTags_UserId",
                table: "ClockSystemTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_BranchId",
                table: "Shifts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_UserId",
                table: "Shifts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchForecastStandard");

            migrationBuilder.DropTable(
                name: "ClockSystemTags");

            migrationBuilder.DropTable(
                name: "Forecast");

            migrationBuilder.DropTable(
                name: "UserAdditionalWorks");

            migrationBuilder.DropTable(
                name: "UserAvailabilities");

            migrationBuilder.DropTable(
                name: "WorkedShifts");

            migrationBuilder.DropTable(
                name: "ForecastStandard");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
