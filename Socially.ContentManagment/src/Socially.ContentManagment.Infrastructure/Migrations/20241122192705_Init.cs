using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Socially.ContentManagment.Infrastructure.Migrations;

  /// <inheritdoc />
  public partial class Init : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.EnsureSchema(
              name: "cm");

          migrationBuilder.CreateTable(
              name: "InboxMessages",
              schema: "cm",
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
                  table.PrimaryKey("PK_InboxMessages", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "Users",
              schema: "cm",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uuid", nullable: false),
                  FirstName = table.Column<string>(type: "text", nullable: false),
                  LastName = table.Column<string>(type: "text", nullable: false),
                  ProfilePictureURL = table.Column<string>(type: "text", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Users", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "Posts",
              schema: "cm",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uuid", nullable: false),
                  UserId = table.Column<Guid>(type: "uuid", nullable: false),
                  Content = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false),
                  MediaURL = table.Column<string>(type: "text", nullable: false),
                  CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                  UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                  Privacy = table.Column<int>(type: "integer", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Posts", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Posts_Users_UserId",
                      column: x => x.UserId,
                      principalSchema: "cm",
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Comments",
              schema: "cm",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uuid", nullable: false),
                  PostId = table.Column<Guid>(type: "uuid", nullable: false),
                  UserId = table.Column<Guid>(type: "uuid", nullable: false),
                  Content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                  CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                  UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Comments", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Comments_Posts_PostId",
                      column: x => x.PostId,
                      principalSchema: "cm",
                      principalTable: "Posts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Shares",
              schema: "cm",
              columns: table => new
              {
                  Id = table.Column<int>(type: "integer", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  PostId = table.Column<Guid>(type: "uuid", nullable: false),
                  UserId = table.Column<Guid>(type: "uuid", nullable: false),
                  Message = table.Column<string>(type: "text", nullable: false),
                  SharedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                  UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Shares", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Shares_Posts_PostId",
                      column: x => x.PostId,
                      principalSchema: "cm",
                      principalTable: "Posts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Likes",
              schema: "cm",
              columns: table => new
              {
                  Id = table.Column<int>(type: "integer", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  UserID = table.Column<Guid>(type: "uuid", nullable: false),
                  PostID = table.Column<Guid>(type: "uuid", nullable: true),
                  CommentID = table.Column<Guid>(type: "uuid", nullable: true),
                  CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                  UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Likes", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Likes_Comments_CommentID",
                      column: x => x.CommentID,
                      principalSchema: "cm",
                      principalTable: "Comments",
                      principalColumn: "Id");
                  table.ForeignKey(
                      name: "FK_Likes_Posts_PostID",
                      column: x => x.PostID,
                      principalSchema: "cm",
                      principalTable: "Posts",
                      principalColumn: "Id");
              });

          migrationBuilder.CreateIndex(
              name: "IX_Comments_PostId",
              schema: "cm",
              table: "Comments",
              column: "PostId");

          migrationBuilder.CreateIndex(
              name: "IX_Likes_CommentID",
              schema: "cm",
              table: "Likes",
              column: "CommentID");

          migrationBuilder.CreateIndex(
              name: "IX_Likes_PostID",
              schema: "cm",
              table: "Likes",
              column: "PostID");

          migrationBuilder.CreateIndex(
              name: "IX_Posts_UserId",
              schema: "cm",
              table: "Posts",
              column: "UserId");

          migrationBuilder.CreateIndex(
              name: "IX_Shares_PostId",
              schema: "cm",
              table: "Shares",
              column: "PostId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "InboxMessages",
              schema: "cm");

          migrationBuilder.DropTable(
              name: "Likes",
              schema: "cm");

          migrationBuilder.DropTable(
              name: "Shares",
              schema: "cm");

          migrationBuilder.DropTable(
              name: "Comments",
              schema: "cm");

          migrationBuilder.DropTable(
              name: "Posts",
              schema: "cm");

          migrationBuilder.DropTable(
              name: "Users",
              schema: "cm");
      }
  }
