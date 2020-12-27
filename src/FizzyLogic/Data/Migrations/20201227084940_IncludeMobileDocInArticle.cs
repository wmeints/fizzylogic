using Microsoft.EntityFrameworkCore.Migrations;

namespace FizzyLogic.Data.Migrations
{
    public partial class IncludeMobileDocInArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileDoc",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileDoc",
                table: "Articles");
        }
    }
}
