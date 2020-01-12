using Microsoft.EntityFrameworkCore.Migrations;

namespace Blazui.Docs.Admin.EFCore.Migrations
{
    public partial class Docs1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangeLog",
                table: "ProductVersions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeLog",
                table: "ProductVersions");
        }
    }
}
