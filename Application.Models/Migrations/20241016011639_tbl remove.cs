using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class tblremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fieldo_ServiceRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fieldo_ServiceRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssignedBy = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Documents = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DomainId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OfferPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_ServiceRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_ServiceRequest_Fieldo_RequestCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Fieldo_RequestCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_ServiceRequest_Fieldo_UserDetails_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
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
                name: "IX_Fieldo_ServiceRequest_CategoryId",
                table: "Fieldo_ServiceRequest",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_ServiceRequest_CreatedBy",
                table: "Fieldo_ServiceRequest",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_ServiceRequest_UpdatedBy",
                table: "Fieldo_ServiceRequest",
                column: "UpdatedBy");
        }
    }
}
