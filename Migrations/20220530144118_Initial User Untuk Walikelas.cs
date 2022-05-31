using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPVUE.Migrations
{
    public partial class InitialUserUntukWalikelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "WaliKelas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaliKelas_UserID",
                table: "WaliKelas",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_WaliKelas_Users_UserID",
                table: "WaliKelas",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaliKelas_Users_UserID",
                table: "WaliKelas");

            migrationBuilder.DropIndex(
                name: "IX_WaliKelas_UserID",
                table: "WaliKelas");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "WaliKelas");
        }
    }
}
