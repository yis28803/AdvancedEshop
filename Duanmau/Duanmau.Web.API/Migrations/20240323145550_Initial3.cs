using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duanmau.Web.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPassCode",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "KhachHangs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPassCode",
                table: "KhachHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "KhachHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
