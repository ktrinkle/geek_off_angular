using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace geek_off_angular.Migrations
{
    /// <inheritdoc />
    public partial class PasswordStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "badge_id",
                table: "team_user");

            migrationBuilder.DropColumn(
                name: "username",
                table: "team_user");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "admin_user",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "round_category",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "text", nullable: false),
                    round_num = table.Column<int>(type: "integer", nullable: false),
                    sub_category_num = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", maxLength: 6, nullable: false),
                    category_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round_category", x => new { x.yevent, x.round_num, x.sub_category_num });
                });

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "password",
                value: null);

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "password",
                value: null);

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "password",
                value: null);

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "password",
                value: null);

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 105L,
                column: "password",
                value: null);

            migrationBuilder.InsertData(
                table: "event_master",
                columns: new[] { "yevent", "event_name", "round2_control", "round3_control", "sel_event" },
                values: new object[] { "t24", "Employee 2021", 0, 3, true });

            migrationBuilder.InsertData(
                table: "round_category",
                columns: new[] { "round_num", "sub_category_num", "yevent", "category_name", "id" },
                values: new object[,]
                {
                    { 3, 10, "e21", "Potent Potables", 1 },
                    { 3, 20, "e21", "Therapists", 2 },
                    { 3, 30, "e21", "An Album", 3 },
                    { 3, 40, "e21", "Potpourri", 4 },
                    { 3, 50, "e21", "Words that begin with G", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "round_category");

            migrationBuilder.DeleteData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "t24");

            migrationBuilder.DropColumn(
                name: "password",
                table: "admin_user");

            migrationBuilder.AddColumn<string>(
                name: "badge_id",
                table: "team_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "team_user",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                columns: new[] { "badge_id", "username" },
                values: new object[] { "285557", "285557@geekoff.onmicrosoft.com" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                columns: new[] { "badge_id", "username" },
                values: new object[] { null, "641903@geekoff.onmicrosoft.com" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 107L,
                columns: new[] { "badge_id", "username" },
                values: new object[] { null, "991023@geekoff.onmicrosoft.com" });
        }
    }
}
