using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PAX.Next.Migrations
{
    public partial class attachpath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "PaxTaskAttachments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "PaxTaskAttachments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "PaxTaskAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "PaxTaskAttachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "PaxTaskAttachments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaxTaskAttachments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PaxTaskAttachments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "PaxTaskAttachments",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PaxTaskAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "PaxTaskAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "PaxTaskAttachments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
