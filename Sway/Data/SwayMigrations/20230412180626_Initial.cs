using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sway.Data.SwayMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    dName = table.Column<string>(type: "TEXT", nullable: false),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    docSentiment = table.Column<string>(type: "TEXT", nullable: true),
                    docPosSentiment = table.Column<double>(type: "REAL", nullable: false),
                    docNegSentiment = table.Column<double>(type: "REAL", nullable: false),
                    docNeutralSentiment = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Phrases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pName = table.Column<string>(type: "TEXT", nullable: false),
                    sentiment = table.Column<double>(type: "REAL", nullable: false),
                    posSentiment = table.Column<double>(type: "REAL", nullable: false),
                    negSentiment = table.Column<double>(type: "REAL", nullable: false),
                    neutralSentiment = table.Column<double>(type: "REAL", nullable: false),
                    DocumentID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrases", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Phrases_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opinion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    oName = table.Column<string>(type: "TEXT", nullable: false),
                    oSentiment = table.Column<double>(type: "REAL", nullable: false),
                    oPosSentiment = table.Column<double>(type: "REAL", nullable: false),
                    oNegSentiment = table.Column<double>(type: "REAL", nullable: false),
                    PhraseID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Opinion_Phrases_PhraseID",
                        column: x => x.PhraseID,
                        principalTable: "Phrases",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    aName = table.Column<string>(type: "TEXT", nullable: false),
                    aSentiment = table.Column<double>(type: "REAL", nullable: false),
                    aPosSentiment = table.Column<double>(type: "REAL", nullable: false),
                    aNegSentiment = table.Column<double>(type: "REAL", nullable: false),
                    OpinionID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Assessment_Opinion_OpinionID",
                        column: x => x.OpinionID,
                        principalTable: "Opinion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_OpinionID",
                table: "Assessment",
                column: "OpinionID");

            migrationBuilder.CreateIndex(
                name: "IX_Opinion_PhraseID",
                table: "Opinion",
                column: "PhraseID");

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_DocumentID",
                table: "Phrases",
                column: "DocumentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropTable(
                name: "Opinion");

            migrationBuilder.DropTable(
                name: "Phrases");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
