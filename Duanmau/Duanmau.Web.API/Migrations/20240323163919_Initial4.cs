using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duanmau.Web.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPassCode",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "NhanViens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPassCode",
                table: "NhanViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "NhanViens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
