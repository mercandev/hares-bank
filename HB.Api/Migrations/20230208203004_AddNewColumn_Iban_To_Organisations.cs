using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnIbanToOrganisations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Organisations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Organisations");
        }
    }
}
