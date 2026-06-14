using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelajarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMataKuliah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MataKuliahs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaMataKuliah = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sks = table.Column<int>(type: "int", nullable: false),
                    MahasiswaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MataKuliahs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MataKuliahs_Mahasiswas_MahasiswaId",
                        column: x => x.MahasiswaId,
                        principalTable: "Mahasiswas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MataKuliahs_MahasiswaId",
                table: "MataKuliahs",
                column: "MahasiswaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MataKuliahs");
        }
    }
}
