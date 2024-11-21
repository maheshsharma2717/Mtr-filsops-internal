using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class IsActivefieldaddedinuserDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsActive",
            //    table: "Fieldo_UserDetails",
            //    type: "tinyint(1)",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Fieldo_UserReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_UserReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_UserReview_Fieldo_UserDetails_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fieldo_UserReview_Fieldo_UserDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_UserReview_AddedBy",
                table: "Fieldo_UserReview",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_UserReview_UserId",
                table: "Fieldo_UserReview",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fieldo_UserReview");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Fieldo_UserDetails");
        }
    }
}
