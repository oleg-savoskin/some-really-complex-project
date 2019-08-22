using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SomeReallyComplexProject.Integration.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationEvents",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    Sender = table.Column<string>(maxLength: 50, nullable: false),
                    EventName = table.Column<string>(maxLength: 100, nullable: false),
                    EventData = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateHandled = table.Column<DateTime>(nullable: true),
                    DatePublished = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEvents", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationEvents");
        }
    }
}
