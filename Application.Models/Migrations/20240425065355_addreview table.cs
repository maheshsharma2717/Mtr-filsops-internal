using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class addreviewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedBy",
                table: "Fieldo_ServiceRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedTo",
                table: "Fieldo_ServiceRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fieldo_Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    ServiceRequestId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    ReviewText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false),
                    Flagged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_Review_Fieldo_ServiceRequest_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "Fieldo_ServiceRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_Review_Fieldo_UserDetails_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_Review_Fieldo_UserDetails_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_Review_Fieldo_UserDetails_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_Review_Fieldo_UserDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_ServiceRequest_AssignedBy",
                table: "Fieldo_ServiceRequest",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_ServiceRequest_AssignedTo",
                table: "Fieldo_ServiceRequest",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Review_CreatedBy",
                table: "Fieldo_Review",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Review_ProviderId",
                table: "Fieldo_Review",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Review_ServiceRequestId",
                table: "Fieldo_Review",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Review_UpdatedBy",
                table: "Fieldo_Review",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_Review_UserId",
                table: "Fieldo_Review",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_ServiceRequest",
                column: "AssignedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedTo",
                table: "Fieldo_ServiceRequest",
                column: "AssignedTo",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedBy",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedTo",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropTable(
                name: "Fieldo_Review");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_ServiceRequest_AssignedBy",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_ServiceRequest_AssignedTo",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Fieldo_ServiceRequest");
        }
    }
}
