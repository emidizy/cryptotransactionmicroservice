using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class CreateTableClientRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestReference = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    WalletAddress = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    CurrencyType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DateRecieved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRequests");
        }
    }
}
