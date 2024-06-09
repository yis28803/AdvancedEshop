using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duanmau.Web.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pass",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "KhachHangs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pass",
                table: "KhachHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "KhachHangs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
