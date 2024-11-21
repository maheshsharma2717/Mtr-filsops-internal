using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeyremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldOps_AdminDeviceToken_Fieldo_UserDetails_UserID",
                table: "FieldOps_AdminDeviceToken");

            migrationBuilder.DropIndex(
                name: "IX_FieldOps_AdminDeviceToken_UserID",
                table: "FieldOps_AdminDeviceToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FieldOps_AdminDeviceToken_UserID",
                table: "FieldOps_AdminDeviceToken",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOps_AdminDeviceToken_Fieldo_UserDetails_UserID",
                table: "FieldOps_AdminDeviceToken",
                column: "UserID",
                principalTable: "Fieldo_UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
