using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class Updaterelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_UserDetailsId",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_UserDetailsId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "Fieldo_Task");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedTo",
                table: "Fieldo_Task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_AssignedBy",
                table: "Fieldo_Task",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_AssignedTo",
                table: "Fieldo_Task",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_CreatedBy",
                table: "Fieldo_Task",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task",
                column: "AssignedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedTo",
                table: "Fieldo_Task",
                column: "AssignedTo",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task",
                column: "CreatedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedTo",
                table: "Fieldo_Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_AssignedTo",
                table: "Fieldo_Task");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_CreatedBy",
                table: "Fieldo_Task");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedTo",
                table: "Fieldo_Task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserDetailsId",
                table: "Fieldo_Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_UserDetailsId",
                table: "Fieldo_Task",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_UserDetailsId",
                table: "Fieldo_Task",
                column: "UserDetailsId",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
