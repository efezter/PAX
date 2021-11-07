using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class taskstaticonurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "TaskStatuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "TaskStatuses");
        }
    }
}
