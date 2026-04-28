using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReferenceManager.Migrations
{
    /// <inheritdoc />
    public partial class StandaloneAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Affiliations = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaperAuthor",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "INTEGER", nullable: false),
                    PapersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperAuthor", x => new { x.AuthorsId, x.PapersId });
                    table.ForeignKey(
                        name: "FK_PaperAuthor_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaperAuthor_Papers_PapersId",
                        column: x => x.PapersId,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaperAuthor_PapersId",
                table: "PaperAuthor",
                column: "PapersId");

            // Migrate existing authors from JSON column to standalone rows
            migrationBuilder.Sql(@"
                INSERT INTO Authors (Name, Email, Affiliations)
                SELECT
                    json_extract(a.value, '$.name'),
                    json_extract(a.value, '$.email'),
                    COALESCE(json_extract(a.value, '$.affiliations'), '[]')
                FROM Papers p, json_each(p.Authors) a
                WHERE p.Authors IS NOT NULL
                  AND json_extract(a.value, '$.name') IS NOT NULL
                GROUP BY lower(json_extract(a.value, '$.name'));
            ");

            migrationBuilder.Sql(@"
                INSERT INTO PaperAuthor (AuthorsId, PapersId)
                SELECT au.Id, p.Id
                FROM Papers p, json_each(p.Authors) a
                JOIN Authors au ON lower(au.Name) = lower(json_extract(a.value, '$.name'))
                WHERE p.Authors IS NOT NULL
                  AND json_extract(a.value, '$.name') IS NOT NULL;
            ");

            migrationBuilder.DropColumn(
                name: "Authors",
                table: "Papers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaperAuthor");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Authors",
                table: "Papers",
                type: "TEXT",
                nullable: true);
        }
    }
}
