using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPVUE.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jurusans",
                columns: table => new
                {
                    JurusanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaJurusan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jurusans", x => x.JurusanID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "WaliKelas",
                columns: table => new
                {
                    WaliKelasID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaWaliKelas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaliKelas", x => x.WaliKelasID);
                });

            migrationBuilder.CreateTable(
                name: "KetuaJurusans",
                columns: table => new
                {
                    KetuaJurusanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaKetuaJurusan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetuaJurusans", x => x.KetuaJurusanID);
                    table.ForeignKey(
                        name: "FK_KetuaJurusans_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kelass",
                columns: table => new
                {
                    KelasID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaKelas = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    JurusanID = table.Column<int>(type: "int", nullable: true),
                    WaliKelasID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kelass", x => x.KelasID);
                    table.ForeignKey(
                        name: "FK_Kelass_Jurusans_JurusanID",
                        column: x => x.JurusanID,
                        principalTable: "Jurusans",
                        principalColumn: "JurusanID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kelass_WaliKelas_WaliKelasID",
                        column: x => x.WaliKelasID,
                        principalTable: "WaliKelas",
                        principalColumn: "WaliKelasID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Siswas",
                columns: table => new
                {
                    SiswaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaSiswa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KelassKelasID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siswas", x => x.SiswaID);
                    table.ForeignKey(
                        name: "FK_Siswas_Kelass_KelassKelasID",
                        column: x => x.KelassKelasID,
                        principalTable: "Kelass",
                        principalColumn: "KelasID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kelass_JurusanID",
                table: "Kelass",
                column: "JurusanID");

            migrationBuilder.CreateIndex(
                name: "IX_Kelass_WaliKelasID",
                table: "Kelass",
                column: "WaliKelasID");

            migrationBuilder.CreateIndex(
                name: "IX_KetuaJurusans_UserID",
                table: "KetuaJurusans",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Siswas_KelassKelasID",
                table: "Siswas",
                column: "KelassKelasID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KetuaJurusans");

            migrationBuilder.DropTable(
                name: "Siswas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Kelass");

            migrationBuilder.DropTable(
                name: "Jurusans");

            migrationBuilder.DropTable(
                name: "WaliKelas");
        }
    }
}
