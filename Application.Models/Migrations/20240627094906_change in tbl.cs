using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class changeintbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_Task_TaskId",
                table: "Fieldo_Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Fieldo_Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "NotificationImage",
                table: "Fieldo_Notifications",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Fieldo_Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Notifications_UserId",
                table: "Fieldo_Notifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_Task_TaskId",
                table: "Fieldo_Notifications",
                column: "TaskId",
                principalTable: "Fieldo_Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_UserDetails_UserId",
                table: "Fieldo_Notifications",
                column: "UserId",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_Task_TaskId",
                table: "Fieldo_Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_UserDetails_UserId",
                table: "Fieldo_Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Notifications_UserId",
                table: "Fieldo_Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationImage",
                table: "Fieldo_Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fieldo_Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Fieldo_Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Notifications_Fieldo_Task_TaskId",
                table: "Fieldo_Notifications",
                column: "TaskId",
                principalTable: "Fieldo_Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
