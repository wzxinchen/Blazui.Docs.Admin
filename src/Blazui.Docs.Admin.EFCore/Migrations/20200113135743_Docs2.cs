using Microsoft.EntityFrameworkCore.Migrations;

namespace Blazui.Docs.Admin.EFCore.Migrations
{
    public partial class Docs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "ProductVersions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Products");
        }
    }
}
