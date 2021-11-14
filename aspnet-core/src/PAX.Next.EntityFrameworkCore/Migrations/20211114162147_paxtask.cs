using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class paxtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaxTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    TaskTypePeriod = table.Column<int>(type: "int", nullable: false),
                    PeriodInterval = table.Column<int>(type: "int", nullable: true),
                    ReporterId = table.Column<long>(type: "bigint", nullable: false),
                    AssigneeId = table.Column<long>(type: "bigint", nullable: true),
                    SeverityId = table.Column<int>(type: "int", nullable: true),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaxTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaxTasks_AbpUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaxTasks_AbpUsers_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaxTasks_Severities_SeverityId",
                        column: x => x.SeverityId,
                        principalTable: "Severities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaxTasks_TaskStatuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaxTasks_AssigneeId",
                table: "PaxTasks",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaxTasks_ReporterId",
                table: "PaxTasks",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_PaxTasks_SeverityId",
                table: "PaxTasks",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_PaxTasks_TaskStatusId",
                table: "PaxTasks",
                column: "TaskStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaxTasks");
        }
    }
}
