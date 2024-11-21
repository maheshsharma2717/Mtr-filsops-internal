using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class fk_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_RequestCategory_CreatedBy",
                table: "Fieldo_RequestCategory",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_RequestCategory_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_RequestCategory",
                column: "CreatedBy",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_RequestCategory_Fieldo_UserDetails_CreatedBy",
                table: "Fieldo_RequestCategory");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_RequestCategory_CreatedBy",
                table: "Fieldo_RequestCategory");
        }
    }
}
