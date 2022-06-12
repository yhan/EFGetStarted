using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFGetStarted.Migrations.Dfa
{
    public partial class InitDfa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DfaDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderLevel = table.Column<int>(type: "integer", nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    ExecutedQty = table.Column<double>(type: "double precision", nullable: false),
                    TargetQty = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DfaDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DfaDetails");
        }
    }
}
