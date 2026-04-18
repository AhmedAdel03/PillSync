using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PillSync.Migrations
{
    /// <inheritdoc />
    public partial class weeklyadherence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicine_Members_MemberId",
                table: "Medicine");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Medicine",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "WeeklyCounterStartDate",
                table: "Medicine",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WeeklyMissedCount",
                table: "Medicine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyTakenCount",
                table: "Medicine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicine_Members_MemberId",
                table: "Medicine",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicine_Members_MemberId",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "WeeklyCounterStartDate",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "WeeklyMissedCount",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "WeeklyTakenCount",
                table: "Medicine");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Medicine",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicine_Members_MemberId",
                table: "Medicine",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
