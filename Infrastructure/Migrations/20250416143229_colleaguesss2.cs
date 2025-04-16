using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class colleaguesss2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameEn",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "FirstNameRu",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "FirstNameTj",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "RoleEn",
                table: "Colleagues");

            migrationBuilder.DropColumn(
                name: "RoleRu",
                table: "Colleagues");

            migrationBuilder.RenameColumn(
                name: "LastNameTj",
                table: "Colleagues",
                newName: "FullNameTj");

            migrationBuilder.RenameColumn(
                name: "LastNameRu",
                table: "Colleagues",
                newName: "FullNameRu");

            migrationBuilder.RenameColumn(
                name: "LastNameEn",
                table: "Colleagues",
                newName: "FullNameEn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullNameTj",
                table: "Colleagues",
                newName: "LastNameTj");

            migrationBuilder.RenameColumn(
                name: "FullNameRu",
                table: "Colleagues",
                newName: "LastNameRu");

            migrationBuilder.RenameColumn(
                name: "FullNameEn",
                table: "Colleagues",
                newName: "LastNameEn");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameEn",
                table: "Colleagues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameRu",
                table: "Colleagues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameTj",
                table: "Colleagues",
                type: "text",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
