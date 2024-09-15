using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class RemoveUnusedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "round1score");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "round1score",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    resp_txt = table.Column<string>(type: "text", nullable: true),
                    row_name = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round1score", x => x.yevent);
                });
        }
    }
}
