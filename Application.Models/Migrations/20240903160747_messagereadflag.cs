﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class messagereadflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Fieldo_Messages",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Fieldo_Messages");
        }
    }
}
