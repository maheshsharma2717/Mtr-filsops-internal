using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class assignedbycolumnnamechangedtoupdatedby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.RenameColumn(
                name: "AssignedBy",
                table: "Fieldo_Task",
                newName: "UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Fieldo_Task_AssignedBy",
                table: "Fieldo_Task",
                newName: "IX_Fieldo_Task_UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_UpdatedBy",
                table: "Fieldo_Task",
                column: "UpdatedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_UpdatedBy",
                table: "Fieldo_Task");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Fieldo_Task",
                newName: "AssignedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Fieldo_Task_UpdatedBy",
                table: "Fieldo_Task",
                newName: "IX_Fieldo_Task_AssignedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task",
                column: "AssignedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }
    }
}
