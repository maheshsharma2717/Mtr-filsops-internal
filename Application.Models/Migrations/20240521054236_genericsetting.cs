using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class genericsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fieldo_GenericSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SettingName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubSettingName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue20_1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue20_2 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue50_1 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue50_2 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue100_1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue100_2 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue250_1 = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextValue250_2 = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextMax = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultTextMax1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefalutInteger1 = table.Column<int>(type: "int", nullable: false),
                    DefalutInteger2 = table.Column<int>(type: "int", nullable: false),
                    DefaultDecimal1 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DefaultDecimal2 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DefaultDateTime1 = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DefaultDateTime2 = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DefaultBool1 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DefaultBool2 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fieldo_GenericSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fieldo_GenericSetting_Fieldo_UserDetails_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fieldo_GenericSetting_Fieldo_UserDetails_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Fieldo_UserDetails",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_GenericSetting_CreatedBy",
                table: "Fieldo_GenericSetting",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Fieldo_GenericSetting_UpdatedBy",
                table: "Fieldo_GenericSetting",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fieldo_GenericSetting");
        }
    }
}
