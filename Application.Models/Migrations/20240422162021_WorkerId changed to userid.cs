using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class WorkerIdchangedtouserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_UserDetails_WorkerId",
                table: "Fieldo_WorkerBankDetails");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Fieldo_WorkerBankDetails",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Fieldo_WorkerBankDetails_WorkerId",
                table: "Fieldo_WorkerBankDetails",
                newName: "IX_Fieldo_WorkerBankDetails_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_UserDetails_UserId",
                table: "Fieldo_WorkerBankDetails",
                column: "UserId",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_UserDetails_UserId",
                table: "Fieldo_WorkerBankDetails");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Fieldo_WorkerBankDetails",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Fieldo_WorkerBankDetails_UserId",
                table: "Fieldo_WorkerBankDetails",
                newName: "IX_Fieldo_WorkerBankDetails_WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_UserDetails_WorkerId",
                table: "Fieldo_WorkerBankDetails",
                column: "WorkerId",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
