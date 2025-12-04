using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class updateDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ma_NhanVien",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_NhanVien",
                table: "DonHang",
                column: "Ma_NhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_NhanVien_Ma_NhanVien",
                table: "DonHang",
                column: "Ma_NhanVien",
                principalTable: "NhanVien",
                principalColumn: "Ma_NhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NhanVien_Ma_NhanVien",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_Ma_NhanVien",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "Ma_NhanVien",
                table: "DonHang");
        }
    }
}
