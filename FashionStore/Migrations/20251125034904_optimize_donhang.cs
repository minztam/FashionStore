using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class optimize_donhang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChi_GiaoHang",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GhiChu_DiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "SoDienThoai_Nhan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "Ten_NguoiNhan",
                table: "DonHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi_GiaoHang",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu_DiaChi",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai_Nhan",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten_NguoiNhan",
                table: "DonHang",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
