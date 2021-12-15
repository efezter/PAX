using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class Added_PaxTaskAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaxTaskAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaxTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaxTaskAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaxTaskAttachments_PaxTasks_PaxTaskId",
                        column: x => x.PaxTaskId,
                        principalTable: "PaxTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaxTaskAttachments_PaxTaskId",
                table: "PaxTaskAttachments",
                column: "PaxTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaxTaskAttachments");
        }
    }
}
