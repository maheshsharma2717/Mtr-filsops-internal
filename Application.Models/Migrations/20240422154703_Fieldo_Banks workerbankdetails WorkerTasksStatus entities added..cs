using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class Fieldo_BanksworkerbankdetailsWorkerTasksStatusentitiesadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task");

            migrationBuilder.AddColumn<float>(
                name: "YearOfExperience",
                table: "Fieldo_UserDetails",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Fieldo_Task",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fieldo_Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_Banks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fieldo_WorkerTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    AssignedTo = table.Column<int>(type: "int", nullable: false),
                    TaskStatus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_WorkerTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_WorkerTasks_Fieldo_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Fieldo_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_WorkerTasks_Fieldo_UserDetails_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_WorkerTasks_Fieldo_UserDetails_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fieldo_WorkerBankDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MiddleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName2 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    OtherBankName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoutingNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_WorkerBankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_WorkerBankDetails_Fieldo_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Fieldo_Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_WorkerBankDetails_Fieldo_UserDetails_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Task_AssignedBy",
                table: "Fieldo_Task",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_WorkerBankDetails_BankId",
                table: "Fieldo_WorkerBankDetails",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_WorkerBankDetails_WorkerId",
                table: "Fieldo_WorkerBankDetails",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_WorkerTasks_AssignedBy",
                table: "Fieldo_WorkerTasks",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_WorkerTasks_AssignedTo",
                table: "Fieldo_WorkerTasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_WorkerTasks_TaskId",
                table: "Fieldo_WorkerTasks",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task",
                column: "AssignedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task",
                column: "CreatedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropTable(
                name: "Fieldo_WorkerBankDetails");

            migrationBuilder.DropTable(
                name: "Fieldo_WorkerTasks");

            migrationBuilder.DropTable(
                name: "Fieldo_Banks");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_Task_AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "YearOfExperience",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Fieldo_Task");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Fieldo_Task",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Fieldo_Task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_Task_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_Task",
                column: "CreatedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }
    }
}
