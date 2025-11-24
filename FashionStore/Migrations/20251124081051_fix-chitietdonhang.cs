using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class fixchitietdonhang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_SanPham_Ma_SanPham",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_TaiKhoan_Ma_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_Ma_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.AlterColumn<int>(
                name: "Ma_TaiKhoan",
                table: "KhachHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Ma_BienThe",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang",
                columns: new[] { "Ma_DonHang", "Ma_BienThe" });

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Ma_TaiKhoan",
                table: "KhachHang",
                column: "Ma_TaiKhoan",
                unique: true,
                filter: "[Ma_TaiKhoan] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_SanPham_Ma_SanPham",
                table: "ChiTietDonHang",
                column: "Ma_SanPham",
                principalTable: "SanPham",
                principalColumn: "Ma_SanPham",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_TaiKhoan_Ma_TaiKhoan",
                table: "KhachHang",
                column: "Ma_TaiKhoan",
                principalTable: "TaiKhoan",
                principalColumn: "Ma_TaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_SanPham_Ma_SanPham",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_TaiKhoan_Ma_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_Ma_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.AlterColumn<int>(
                name: "Ma_TaiKhoan",
                table: "KhachHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ma_BienThe",
                table: "ChiTietDonHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang",
                columns: new[] { "Ma_DonHang", "Ma_SanPham" });

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Ma_TaiKhoan",
                table: "KhachHang",
                column: "Ma_TaiKhoan",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_SanPham_Ma_SanPham",
                table: "ChiTietDonHang",
                column: "Ma_SanPham",
                principalTable: "SanPham",
                principalColumn: "Ma_SanPham",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_TaiKhoan_Ma_TaiKhoan",
                table: "KhachHang",
                column: "Ma_TaiKhoan",
                principalTable: "TaiKhoan",
                principalColumn: "Ma_TaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
