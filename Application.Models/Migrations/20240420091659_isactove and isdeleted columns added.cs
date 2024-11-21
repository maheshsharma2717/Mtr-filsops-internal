using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class isactoveandisdeletedcolumnsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Fieldo_ServiceRequest",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Fieldo_ServiceRequest",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Fieldo_RequestCategory",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Fieldo_RequestCategory",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Fieldo_RequestCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Fieldo_RequestCategory");
        }
    }
}
