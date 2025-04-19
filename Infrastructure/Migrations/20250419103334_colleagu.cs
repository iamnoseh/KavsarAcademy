using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class colleagu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleTj",
                table: "Colleagues");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Colleagues",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Colleagues");

            migrationBuilder.AddColumn<string>(
                name: "RoleTj",
                table: "Colleagues",
                type: "text",
                nullable: true);
        }
    }
}
