using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStarted.Migrations.Dfa
{
    public partial class AddOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "DfaDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DfaDetails");
        }
    }
}
