using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class add_diachidonhang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Ma_DiaChi",
                table: "DonHang",
                type: "int",
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

            migrationBuilder.CreateTable(
                name: "DiaChiGiaoHangs",
                columns: table => new
                {
                    Ma_DiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen_NguoiNhan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi_ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChiGiaoHangs", x => x.Ma_DiaChi);
                    table.ForeignKey(
                        name: "FK_DiaChiGiaoHangs_KhachHang_Ma_KhachHang",
                        column: x => x.Ma_KhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Ma_KhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_DiaChi",
                table: "DonHang",
                column: "Ma_DiaChi");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiGiaoHangs_Ma_KhachHang",
                table: "DiaChiGiaoHangs",
                column: "Ma_KhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "DonHang",
                column: "Ma_DiaChi",
                principalTable: "DiaChiGiaoHangs",
                principalColumn: "Ma_DiaChi",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_DiaChiGiaoHangs_Ma_DiaChi",
                table: "DonHang");

            migrationBuilder.DropTable(
                name: "DiaChiGiaoHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_Ma_DiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "DiaChi_GiaoHang",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GhiChu_DiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "Ma_DiaChi",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "SoDienThoai_Nhan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "Ten_NguoiNhan",
                table: "DonHang");
        }
    }
}
