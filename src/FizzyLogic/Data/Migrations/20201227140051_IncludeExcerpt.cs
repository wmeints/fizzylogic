using Microsoft.EntityFrameworkCore.Migrations;

namespace FizzyLogic.Data.Migrations
{
    public partial class IncludeExcerpt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "Articles");
        }
    }
}
