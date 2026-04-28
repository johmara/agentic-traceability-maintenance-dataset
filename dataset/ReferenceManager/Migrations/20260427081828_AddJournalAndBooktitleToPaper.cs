using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReferenceManager.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalAndBooktitleToPaper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Booktitle",
                table: "Papers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Journal",
                table: "Papers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booktitle",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "Journal",
                table: "Papers");
        }
    }
}
