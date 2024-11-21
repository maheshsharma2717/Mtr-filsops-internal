using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class modifiedpaymenttbleandtasktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Payments_Fieldo_Task_TaskId",
                table: "Fieldo_Payments");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Payments_TaskId",
                table: "Fieldo_Payments");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Fieldo_Payments");

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "Fieldo_Task",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Fieldo_Task",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "Fieldo_Task",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "Fieldo_Task");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Fieldo_Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Payments_TaskId",
                table: "Fieldo_Payments",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Payments_Fieldo_Task_TaskId",
                table: "Fieldo_Payments",
                column: "TaskId",
                principalTable: "Fieldo_Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
