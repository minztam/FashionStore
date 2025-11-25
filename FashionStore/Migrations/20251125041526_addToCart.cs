using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class addToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ma_DiaChi",
                table: "GioHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Ma_DiaChi",
                table: "DonHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                column: "DiaChiGiaoHangMa_DiaChi");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang",
                column: "DiaChiGiaoHangMa_DiaChi",
                principalTable: "DiaChiGiaoHangs",
                principalColumn: "Ma_DiaChi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHangMa_DiaChi",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "Ma_DiaChi",
                table: "GioHang");

            migrationBuilder.AlterColumn<int>(
                name: "Ma_DiaChi",
                table: "DonHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
