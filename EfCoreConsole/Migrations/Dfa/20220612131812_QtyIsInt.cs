using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStarted.Migrations.Dfa
{
    public partial class QtyIsInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TargetQty",
                table: "DfaDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutedQty",
                table: "DfaDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TargetQty",
                table: "DfaDetails",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "ExecutedQty",
                table: "DfaDetails",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
