using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class createdbycolumnforadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByAdminUserId",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByAdminUserName",
                table: "Fieldo_Task",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByAdminUserType",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaskCreatedByAdmin",
                table: "Fieldo_Task",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByAdminUserId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CreatedByAdminUserName",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CreatedByAdminUserType",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "IsTaskCreatedByAdmin",
                table: "Fieldo_Task");
        }
    }
}
