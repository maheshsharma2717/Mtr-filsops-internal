using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class fk_remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_RequestCategory_CreatedBy",
                table: "Fieldo_RequestCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_RequestCategory_CreatedBy",
                table: "Fieldo_RequestCategory",
                column: "CreatedBy");

           
        }
    }
}
