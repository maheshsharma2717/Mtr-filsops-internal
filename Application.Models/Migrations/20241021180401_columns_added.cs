using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class columns_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedByUserId",
                table: "Fieldo_WorkerTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedByUserName",
                table: "Fieldo_WorkerTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedByAdminUserType",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedByUserId",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedByUserName",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Fieldo_WorkerTasks");

            migrationBuilder.DropColumn(
                name: "AssignedByUserName",
                table: "Fieldo_WorkerTasks");

            migrationBuilder.DropColumn(
                name: "AssignedByAdminUserType",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "AssignedByUserName",
                table: "Fieldo_Task");
        }
    }
}
