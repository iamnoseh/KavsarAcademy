using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class colleague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleEn",
                table: "Colleagues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleRu",
                table: "Colleagues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleTj",
                table: "Colleagues",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleEn",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "RoleRu",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "RoleTj",
                table: "Colleagues");
        }
    }
}
