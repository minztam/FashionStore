using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Cascade_Cycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_Ma_KhachHang",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_Ma_DiaChi",
                table: "GioHang",
                column: "Ma_DiaChi");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_Ma_KhachHang",
                table: "GioHang",
                column: "Ma_KhachHang",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "GioHang",
                column: "Ma_DiaChi",
                principalTable: "DiaChiGiaoHangs",
                principalColumn: "Ma_DiaChi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_Ma_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_Ma_KhachHang",
                table: "GioHang");

            migrationBuilder.AddColumn<int>(
                name: "DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                column: "DiaChiGiaoHangMa_DiaChi");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_Ma_KhachHang",
                table: "GioHang",
                column: "Ma_KhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                column: "DiaChiGiaoHangMa_DiaChi",
                principalTable: "DiaChiGiaoHangs",
                principalColumn: "Ma_DiaChi");
        }
    }
}
