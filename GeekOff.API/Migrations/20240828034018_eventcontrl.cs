using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace geek_off_angular.Migrations
{
    /// <inheritdoc />
    public partial class eventcontrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "round2_control",
                table: "event_master",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "round3_control",
                table: "event_master",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "e19",
                columns: new[] { "round2_control", "round3_control" },
                values: new object[] { 0, 3 });

            migrationBuilder.UpdateData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "e21",
                columns: new[] { "round2_control", "round3_control" },
                values: new object[] { 0, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "round2_control",
                table: "event_master");

            migrationBuilder.DropColumn(
                name: "round3_control",
                table: "event_master");
        }
    }
}
