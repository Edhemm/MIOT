using Microsoft.EntityFrameworkCore.Migrations;

namespace Irma.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uredjaji",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    deviceId = table.Column<int>(nullable: false),
                    listaLokacija = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uredjaji", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "XMLDocs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dokument = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XMLDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Senzori",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    senzorId = table.Column<int>(nullable: false),
                    imeSenzora = table.Column<string>(nullable: true),
                    tipSenzora = table.Column<string>(nullable: true),
                    Uredjajid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senzori", x => x.id);
                    table.ForeignKey(
                        name: "FK_Senzori_Uredjaji_Uredjajid",
                        column: x => x.Uredjajid,
                        principalTable: "Uredjaji",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mjerenja",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vrijemeMjerenja = table.Column<string>(nullable: true),
                    minVrijednost = table.Column<string>(nullable: true),
                    maxVrijednost = table.Column<string>(nullable: true),
                    alarm = table.Column<string>(nullable: true),
                    vrijednostMjerenja = table.Column<string>(nullable: true),
                    validnostMjeranja = table.Column<string>(nullable: true),
                    Senzorid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mjerenja", x => x.id);
                    table.ForeignKey(
                        name: "FK_Mjerenja_Senzori_Senzorid",
                        column: x => x.Senzorid,
                        principalTable: "Senzori",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mjerenja_Senzorid",
                table: "Mjerenja",
                column: "Senzorid");

            migrationBuilder.CreateIndex(
                name: "IX_Senzori_Uredjajid",
                table: "Senzori",
                column: "Uredjajid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mjerenja");

            migrationBuilder.DropTable(
                name: "XMLDocs");

            migrationBuilder.DropTable(
                name: "Senzori");

            migrationBuilder.DropTable(
                name: "Uredjaji");
        }
    }
}
