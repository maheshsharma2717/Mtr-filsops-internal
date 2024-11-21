using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class viewstatustimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedTime",
                table: "Fieldo_Task",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "Fieldo_Task",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ViewedTime",
                table: "Fieldo_Task",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkCompleteTime",
                table: "Fieldo_Task",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkStartTime",
                table: "Fieldo_Task",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_ViewedBy",
                table: "Fieldo_Task",
                column: "ViewedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_ViewedBy",
                table: "Fieldo_Task",
                column: "ViewedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_ViewedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_ViewedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "AssignedTime",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "ViewedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "ViewedTime",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "WorkCompleteTime",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "WorkStartTime",
                table: "Fieldo_Task");
        }
    }
}
