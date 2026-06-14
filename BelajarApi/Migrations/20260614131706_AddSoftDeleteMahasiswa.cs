using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelajarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteMahasiswa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Mahasiswas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Mahasiswas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Mahasiswas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Mahasiswas");
        }
    }
}
