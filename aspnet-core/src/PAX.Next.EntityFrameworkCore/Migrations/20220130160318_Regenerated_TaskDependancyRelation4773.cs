using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class Regenerated_TaskDependancyRelation4773 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskDependancyRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaxTaskId = table.Column<int>(type: "int", nullable: false),
                    DependOnTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDependancyRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskDependancyRelations_PaxTasks_DependOnTaskId",
                        column: x => x.DependOnTaskId,
                        principalTable: "PaxTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskDependancyRelations_PaxTasks_PaxTaskId",
                        column: x => x.PaxTaskId,
                        principalTable: "PaxTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDependancyRelations_DependOnTaskId",
                table: "TaskDependancyRelations",
                column: "DependOnTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDependancyRelations_PaxTaskId",
                table: "TaskDependancyRelations",
                column: "PaxTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDependancyRelations");
        }
    }
}
