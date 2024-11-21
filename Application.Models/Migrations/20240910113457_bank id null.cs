using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class bankidnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_Banks_BankId",
                table: "Fieldo_WorkerBankDetails");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "Fieldo_WorkerBankDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_Banks_BankId",
                table: "Fieldo_WorkerBankDetails",
                column: "BankId",
                principalTable: "Fieldo_Banks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_Banks_BankId",
                table: "Fieldo_WorkerBankDetails");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "Fieldo_WorkerBankDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_WorkerBankDetails_Fieldo_Banks_BankId",
                table: "Fieldo_WorkerBankDetails",
                column: "BankId",
                principalTable: "Fieldo_Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
