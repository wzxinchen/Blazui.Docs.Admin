using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Blazui.Docs.Admin.EFCore.Migrations
{
    public partial class Components : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentGenericParameter");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVersionId",
                table: "Components",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Components",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComponentVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<int>(nullable: false),
                    ProductVersionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportedTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Namespace = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsGeneric = table.Column<bool>(nullable: false),
                    ProductVersionId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportedProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    PropertyType = table.Column<string>(nullable: true),
                    IsComponentParameter = table.Column<bool>(nullable: false),
                    IsComponentCascadingParameter = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExportedTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportedTypeGenericParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExportedTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedTypeGenericParameters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentVersions");

            migrationBuilder.DropTable(
                name: "ExportedProperties");

            migrationBuilder.DropTable(
                name: "ExportedTypeGenericParameters");

            migrationBuilder.DropTable(
                name: "ExportedTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Components");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVersionId",
                table: "Components",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ComponentGenericParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentGenericParameter", x => x.Id);
                });
        }
    }
}
