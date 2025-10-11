using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace geek_off_angular.Migrations
{
    /// <inheritdoc />
    public partial class AdjustAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "admin_user",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_guid",
                table: "admin_user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "user_guid",
                value: Guid.NewGuid());

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "user_guid",
                value: Guid.NewGuid());

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "user_guid",
                value: Guid.NewGuid());

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "user_guid",
                value: Guid.NewGuid());

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 105L,
                column: "user_guid",
                value: Guid.NewGuid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_guid",
                table: "admin_user");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "admin_user",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);
        }
    }
}
