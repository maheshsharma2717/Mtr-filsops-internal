using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class viewedbyadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewedByAdminUserType",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewedByUserId",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewedByUserName",
                table: "Fieldo_Task",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewedByAdminUserType",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "ViewedByUserId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "ViewedByUserName",
                table: "Fieldo_Task");
        }
    }
}
