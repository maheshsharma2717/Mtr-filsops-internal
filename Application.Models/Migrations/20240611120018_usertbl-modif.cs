using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class usertblmodif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Fieldo_UserDetails",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Fieldo_UserDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Fieldo_UserDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Fieldo_UserDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "Fieldo_UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_UserDetails_ServiceCategoryId",
                table: "Fieldo_UserDetails",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fieldo_UserDetails_Fieldo_RequestCategory_ServiceCategoryId",
                table: "Fieldo_UserDetails",
                column: "ServiceCategoryId",
                principalTable: "Fieldo_RequestCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fieldo_UserDetails_Fieldo_RequestCategory_ServiceCategoryId",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_Fieldo_UserDetails_ServiceCategoryId",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "Fieldo_UserDetails");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Fieldo_UserDetails",
                newName: "Name");
        }
    }
}
