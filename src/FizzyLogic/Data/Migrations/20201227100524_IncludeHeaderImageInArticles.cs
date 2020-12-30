﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace FizzyLogic.Data.Migrations
{
    public partial class IncludeHeaderImageInArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                table: "Articles");
        }
    }
}
