using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace geek_off_angular.Migrations
{
    public partial class AdminChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 108L);

            migrationBuilder.DropColumn(
                name: "admin_flag",
                table: "team_user");

            migrationBuilder.AddColumn<DateTime>(
                name: "login_time",
                table: "team_reference",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "admin_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    admin_name = table.Column<string>(type: "text", nullable: true),
                    login_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_user", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "admin_user",
                columns: new[] { "id", "admin_name", "login_time", "username" },
                values: new object[,]
                {
                    { 100L, "Kevin Trinkle", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "362525" },
                    { 101L, "Kristin Russell", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "446792" },
                    { 102L, "Diyalo Manral", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "226250" },
                    { 103L, "Dan Mullings", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "288132" },
                    { 105L, "Jay Cox", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jaycox" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_user");

            migrationBuilder.DropColumn(
                name: "login_time",
                table: "team_reference");

            migrationBuilder.AddColumn<bool>(
                name: "admin_flag",
                table: "team_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                column: "admin_flag",
                value: true);

            migrationBuilder.InsertData(
                table: "team_user",
                columns: new[] { "id", "admin_flag", "badge_id", "login_time", "player_name", "player_num", "session_id", "team_num", "username", "workgroup_name", "yevent" },
                values: new object[,]
                {
                    { 100L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kevin Trinkle", null, new Guid("00000000-0000-0000-0000-000000000000"), 0, "362525@geekoff.onmicrosoft.com", null, "e21" },
                    { 101L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kristin Russell", null, new Guid("00000000-0000-0000-0000-000000000000"), 0, "446792@geekoff.onmicrosoft.com", null, "e21" },
                    { 102L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diyalo Manral", null, new Guid("00000000-0000-0000-0000-000000000000"), 0, "226250@geekoff.onmicrosoft.com", null, "e21" },
                    { 103L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dan Mullings", null, new Guid("00000000-0000-0000-0000-000000000000"), 0, "288132@geekoff.onmicrosoft.com", null, "e21" },
                    { 105L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jay Cox", null, new Guid("00000000-0000-0000-0000-000000000000"), 0, "jay.cox_aa.com#EXT#@geekoff.onmicrosoft.com", null, "e21" },
                    { 108L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kevin Trinkle", 0, new Guid("00000000-0000-0000-0000-000000000000"), 0, "kevin.trinkle_aa.com#EXT#@geekoff.onmicrosoft.com", "Information Technology", "e21" }
                });
        }
    }
}
