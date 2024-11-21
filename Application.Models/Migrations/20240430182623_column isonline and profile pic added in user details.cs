using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class columnisonlineandprofilepicaddedinuserdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Fieldo_UserDetails",
                newName: "ProfileUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Fieldo_UserDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Fieldo_UserDetails");

            migrationBuilder.RenameColumn(
                name: "ProfileUrl",
                table: "Fieldo_UserDetails",
                newName: "Status");
        }
    }
}
