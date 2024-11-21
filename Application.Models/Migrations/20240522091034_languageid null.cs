using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class languageidnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LanguageId",
                table: "Fieldo_UserDetails",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fieldo_UserDetails",
                keyColumn: "LanguageId",
                keyValue: null,
                column: "LanguageId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageId",
                table: "Fieldo_UserDetails",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
