using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.UserManagment.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingSchemaAndTheOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "um");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "um");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokens",
                newSchema: "um");

            migrationBuilder.CreateTable(
                name: "outboxMessages",
                schema: "um",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    OccuredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outboxMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outboxMessages",
                schema: "um");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "um",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "um",
                newName: "RefreshTokens");
        }
    }
}
