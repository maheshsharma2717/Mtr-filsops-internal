using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class cancelledtaskuserdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancelledBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancelledByAdminUserId",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledByAdminUserName",
                table: "Fieldo_Task",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CancelledByAdminUserType",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaskCancelledByAdmin",
                table: "Fieldo_Task",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_CancelledBy",
                table: "Fieldo_Task",
                column: "CancelledBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_RequestCategory_CancelledBy",
                table: "Fieldo_Task",
                column: "CancelledBy",
                principalTable: "Fieldo_RequestCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_RequestCategory_CancelledBy",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_CancelledBy",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CancelledByAdminUserId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CancelledByAdminUserName",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "CancelledByAdminUserType",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "IsTaskCancelledByAdmin",
                table: "Fieldo_Task");
        }
    }
}
