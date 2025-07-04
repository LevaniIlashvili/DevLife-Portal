using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DevLifePortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBugChaseProfileDefineRelationshipToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeCasinoProfiles_UserId",
                table: "CodeCasinoProfiles");

            migrationBuilder.CreateTable(
                name: "BugChaseProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugChaseProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BugChaseProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeCasinoProfiles_UserId",
                table: "CodeCasinoProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BugChaseProfiles_UserId",
                table: "BugChaseProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugChaseProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CodeCasinoProfiles_UserId",
                table: "CodeCasinoProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_CodeCasinoProfiles_UserId",
                table: "CodeCasinoProfiles",
                column: "UserId");
        }
    }
}
