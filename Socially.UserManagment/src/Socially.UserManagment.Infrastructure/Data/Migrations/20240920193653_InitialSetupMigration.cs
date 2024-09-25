using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.UserManagment.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitialSetupMigration : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "Users",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uuid", nullable: false),
          Username = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
          Email = table.Column<string>(type: "text", nullable: false),
          PasswordHash = table.Column<string>(type: "text", nullable: false),
          CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
          LastLoginAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
          IsActive = table.Column<bool>(type: "boolean", nullable: false),
          Bio = table.Column<string>(type: "text", nullable: false),
          FirstName = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
          LastName = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
          ProfilePictureURL = table.Column<string>(type: "text", nullable: true),
          CoverPhotoURL = table.Column<string>(type: "text", nullable: true),
          DateOfBirth = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
          Gender = table.Column<bool>(type: "boolean", nullable: false),
          UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Users", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "RefreshTokens",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uuid", nullable: false),
          UserId = table.Column<Guid>(type: "uuid", nullable: false),
          Token = table.Column<string>(type: "text", nullable: false),
          Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
          IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
          Family = table.Column<string>(type: "text", nullable: false),
          ParentTokenId = table.Column<Guid>(type: "uuid", nullable: true),
          RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_RefreshTokens", x => x.Id);
          table.ForeignKey(
                    name: "FK_RefreshTokens_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_RefreshTokens_Token",
        table: "RefreshTokens",
        column: "Token",
        unique: true);

    migrationBuilder.CreateIndex(
        name: "IX_RefreshTokens_UserId",
        table: "RefreshTokens",
        column: "UserId");

    migrationBuilder.CreateIndex(
        name: "IX_Users_Username",
        table: "Users",
        column: "Username",
        unique: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "RefreshTokens");

    migrationBuilder.DropTable(
        name: "Users");
  }
}
