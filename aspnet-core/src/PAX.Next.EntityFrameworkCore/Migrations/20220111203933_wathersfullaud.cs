using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class wathersfullaud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Watchers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Watchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Watchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Watchers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Watchers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Watchers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Watchers",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Watchers");
        }
    }
}
