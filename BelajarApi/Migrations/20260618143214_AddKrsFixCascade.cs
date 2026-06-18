using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelajarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddKrsFixCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KrsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MahasiswaId = table.Column<int>(type: "int", nullable: false),
                    MataKuliahId = table.Column<int>(type: "int", nullable: false),
                    TanggalAmbil = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrsList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KrsList_Mahasiswas_MahasiswaId",
                        column: x => x.MahasiswaId,
                        principalTable: "Mahasiswas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KrsList_MataKuliahs_MataKuliahId",
                        column: x => x.MataKuliahId,
                        principalTable: "MataKuliahs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KrsList_MahasiswaId",
                table: "KrsList",
                column: "MahasiswaId");

            migrationBuilder.CreateIndex(
                name: "IX_KrsList_MataKuliahId",
                table: "KrsList",
                column: "MataKuliahId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KrsList");
        }
    }
}
