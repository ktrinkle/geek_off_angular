using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class TeamReferencePopulate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "team_reference",
                columns: new[] { "team_no", "yevent", "dollarraised", "member1", "member2", "teamname", "workgroup1", "workgroup2" },
                values: new object[] { 1, "e21", 1000m, "Grant Hill", "Brandon Heath", "Go Aggies", "IT", "IT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "team_reference",
                keyColumns: new[] { "team_no", "yevent" },
                keyValues: new object[] { 1, "e21" });
        }
    }
}
