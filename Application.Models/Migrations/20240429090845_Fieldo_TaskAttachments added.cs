using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class Fieldo_TaskAttachmentsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fieldo_TaskAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_TaskAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_TaskAttachments_Fieldo_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Fieldo_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_TaskAttachments_Fieldo_UserDetails_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_TaskAttachments_AddedBy",
                table: "Fieldo_TaskAttachments",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_TaskAttachments_TaskId",
                table: "Fieldo_TaskAttachments",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fieldo_TaskAttachments");
        }
    }
}
