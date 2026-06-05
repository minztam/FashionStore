using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class Fix_cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_Ma_DiaChi",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "Ma_DiaChi",
                table: "GioHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ma_DiaChi",
                table: "GioHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_Ma_DiaChi",
                table: "GioHang",
                column: "Ma_DiaChi");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "GioHang",
                column: "Ma_DiaChi",
                principalTable: "DiaChiGiaoHangs",
                principalColumn: "Ma_DiaChi");
        }
    }
}
