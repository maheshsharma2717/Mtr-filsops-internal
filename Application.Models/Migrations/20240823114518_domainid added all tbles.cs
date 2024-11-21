using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class domainidaddedalltbles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_WorkerTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_WorkerBankDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Wallet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_UserReview",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_TaskStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_TaskAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Task",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OfferPrice",
                table: "Fieldo_ServiceRequest",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_ServiceRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_RequestCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultDecimal2",
                table: "Fieldo_GenericSetting",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultDecimal1",
                table: "Fieldo_GenericSetting",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_GenericSetting",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_EmailTemplate",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Banks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "Fieldo_Address",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_WorkerTasks");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_WorkerBankDetails");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Wallet");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_UserReview");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_UserDetails");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_TaskStatus");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_TaskAttachments");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Task");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_ServiceRequest");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Roles");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_RequestCategory");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Payments");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Notifications");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Messages");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_GenericSetting");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_EmailTemplate");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Banks");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Fieldo_Address");

            migrationBuilder.AlterColumn<decimal>(
                name: "OfferPrice",
                table: "Fieldo_ServiceRequest",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultDecimal2",
                table: "Fieldo_GenericSetting",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultDecimal1",
                table: "Fieldo_GenericSetting",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
