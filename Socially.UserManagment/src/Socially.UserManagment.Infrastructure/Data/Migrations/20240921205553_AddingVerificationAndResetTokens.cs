using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socially.ContentManagment.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddingVerificationAndResetTokens : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.AddColumn<bool>(
        name: "IsEmailVerified",
        table: "Users",
        type: "boolean",
        nullable: false,
        defaultValue: false);

    migrationBuilder.AddColumn<string>(
        name: "ResetPasswordToken",
        table: "Users",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<DateTimeOffset>(
        name: "ResetTokenGeneratedAt",
        table: "Users",
        type: "timestamp with time zone",
        nullable: true);

    migrationBuilder.AddColumn<DateTimeOffset>(
        name: "TokenGeneratedAt",
        table: "Users",
        type: "timestamp with time zone",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "VerificationToken",
        table: "Users",
        type: "text",
        nullable: true);

    migrationBuilder.CreateIndex(
        name: "IX_Users_VerificationToken",
        table: "Users",
        column: "VerificationToken",
        unique: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropIndex(
        name: "IX_Users_VerificationToken",
        table: "Users");

    migrationBuilder.DropColumn(
        name: "IsEmailVerified",
        table: "Users");

    migrationBuilder.DropColumn(
        name: "ResetPasswordToken",
        table: "Users");

    migrationBuilder.DropColumn(
        name: "ResetTokenGeneratedAt",
        table: "Users");

    migrationBuilder.DropColumn(
        name: "TokenGeneratedAt",
        table: "Users");

    migrationBuilder.DropColumn(
        name: "VerificationToken",
        table: "Users");
  }
}
