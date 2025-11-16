using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    Ma_DanhMuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten_DanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma_DanhMucCha = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMuc", x => x.Ma_DanhMuc);
                    table.ForeignKey(
                        name: "FK_DanhMuc_DanhMuc_Ma_DanhMucCha",
                        column: x => x.Ma_DanhMucCha,
                        principalTable: "DanhMuc",
                        principalColumn: "Ma_DanhMuc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    Ma_PhuongThuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_PhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.Ma_PhuongThuc);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    Ma_VaiTro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten_VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.Ma_VaiTro);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Ma_Voucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Giam_PhanTram = table.Column<int>(type: "int", nullable: true),
                    Giam_Tien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaTri_ToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    So_LanDung = table.Column<int>(type: "int", nullable: true),
                    Ngay_BatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ngay_KetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Ma_Voucher);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten_SanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma_DanhMuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mo_Ta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gia_Giam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    Mau_Sac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kich_Thuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.Ma_SanPham);
                    table.ForeignKey(
                        name: "FK_SanPham_DanhMuc_Ma_DanhMuc",
                        column: x => x.Ma_DanhMuc,
                        principalTable: "DanhMuc",
                        principalColumn: "Ma_DanhMuc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    Ma_TaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_DangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mat_Khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma_VaiTro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Da_XacThuc = table.Column<bool>(type: "bit", nullable: false),
                    Ma_XacThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Han_XacThuc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.Ma_TaiKhoan);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_VaiTro_Ma_VaiTro",
                        column: x => x.Ma_VaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "Ma_VaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhSanPham",
                columns: table => new
                {
                    Ma_HinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPham", x => x.Ma_HinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPham_SanPham_Ma_SanPham",
                        column: x => x.Ma_SanPham,
                        principalTable: "SanPham",
                        principalColumn: "Ma_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_TaiKhoan = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hinh_Anh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.Ma_KhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_TaiKhoan_Ma_TaiKhoan",
                        column: x => x.Ma_TaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "Ma_TaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    Ma_NhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_TaiKhoan = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hinh_Anh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.Ma_NhanVien);
                    table.ForeignKey(
                        name: "FK_NhanVien_TaiKhoan_Ma_TaiKhoan",
                        column: x => x.Ma_TaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "Ma_TaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    Ma_DonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false),
                    Ngay_Dat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tong_Tien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Trang_Thai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma_PhuongThuc = table.Column<int>(type: "int", nullable: false),
                    Ma_Voucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KhachHangMa_KhachHang = table.Column<int>(type: "int", nullable: true),
                    PhuongThucThanhToanMa_PhuongThuc = table.Column<int>(type: "int", nullable: true),
                    VoucherMa_Voucher = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.Ma_DonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_KhachHang_KhachHangMa_KhachHang",
                        column: x => x.KhachHangMa_KhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Ma_KhachHang");
                    table.ForeignKey(
                        name: "FK_DonHangs_PhuongThucThanhToans_PhuongThucThanhToanMa_PhuongThuc",
                        column: x => x.PhuongThucThanhToanMa_PhuongThuc,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "Ma_PhuongThuc");
                    table.ForeignKey(
                        name: "FK_DonHangs_Vouchers_VoucherMa_Voucher",
                        column: x => x.VoucherMa_Voucher,
                        principalTable: "Vouchers",
                        principalColumn: "Ma_Voucher");
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    Ma_GioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false),
                    KhachHangMa_KhachHang = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.Ma_GioHang);
                    table.ForeignKey(
                        name: "FK_GioHangs_KhachHang_KhachHangMa_KhachHang",
                        column: x => x.KhachHangMa_KhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Ma_KhachHang");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    Ma_DonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => new { x.Ma_DonHang, x.Ma_SanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_Ma_DonHang",
                        column: x => x.Ma_DonHang,
                        principalTable: "DonHangs",
                        principalColumn: "Ma_DonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPham_Ma_SanPham",
                        column: x => x.Ma_SanPham,
                        principalTable: "SanPham",
                        principalColumn: "Ma_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHangs",
                columns: table => new
                {
                    Ma_GioHang = table.Column<int>(type: "int", nullable: false),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHangs", x => new { x.Ma_GioHang, x.Ma_SanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_GioHangs_Ma_GioHang",
                        column: x => x.Ma_GioHang,
                        principalTable: "GioHangs",
                        principalColumn: "Ma_GioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_SanPham_Ma_SanPham",
                        column: x => x.Ma_SanPham,
                        principalTable: "SanPham",
                        principalColumn: "Ma_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DanhMuc",
                columns: new[] { "Ma_DanhMuc", "Ma_DanhMucCha", "Ten_DanhMuc", "Trang_Thai" },
                values: new object[,]
                {
                    { "DM001", null, "Thời trang nam", true },
                    { "DM002", null, "Thời trang nữ", true }
                });

            migrationBuilder.InsertData(
                table: "VaiTro",
                columns: new[] { "Ma_VaiTro", "Ten_VaiTro" },
                values: new object[,]
                {
                    { "1111-1111-1111-1111", "Admin" },
                    { "1111-2222-1111-2222", "Nhân viên bán hàng" },
                    { "1111-3333-1111-3333", "Nhân viên kho" },
                    { "2222-2222-2222-2222", "Khách hàng" }
                });

            migrationBuilder.InsertData(
                table: "DanhMuc",
                columns: new[] { "Ma_DanhMuc", "Ma_DanhMucCha", "Ten_DanhMuc", "Trang_Thai" },
                values: new object[,]
                {
                    { "DM001A", "DM001", "Áo nam", true },
                    { "DM001B", "DM001", "Quần nam", true },
                    { "DM002A", "DM002", "Áo nữ", true },
                    { "DM002B", "DM002", "Váy nữ", true }
                });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "Ma_SanPham", "Gia", "Gia_Giam", "Kich_Thuoc", "Ma_DanhMuc", "Mau_Sac", "Mo_Ta", "So_Luong", "Ten_SanPham", "Trang_Thai" },
                values: new object[,]
                {
                    { "SP001", 450000m, 399000m, "M, L, XL", "DM001A", "Trắng", "Áo sơ mi vải Oxford cao cấp, thoáng mát, phong cách công sở trẻ trung.", 150, "Áo Sơ Mi Oxford Nam", true },
                    { "SP002", 780000m, null, "S, M, L", "DM002B", "Vàng, Xanh", "Váy maxi chất liệu lụa mềm mại, họa tiết hoa nhí, thích hợp đi biển hoặc dạo phố.", 80, "Váy Maxi Lụa Hoa Nhí", true },
                    { "SP003", 550000m, 450000m, "28, 29, 30, 31, 32", "DM001B", "Đen", "Quần jeans co giãn nhẹ, form slimfit hiện đại, dễ phối đồ.", 120, "Quần Jeans Slimfit Đen", true },
                    { "SP004", 320000m, null, "Freesize", "DM002A", "Be, Đỏ", "Áo len mỏng, kiểu dáng croptop, phong cách Hàn Quốc.", 95, "Áo Len Croptop Tay Dài", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_Ma_SanPham",
                table: "ChiTietDonHangs",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_Ma_SanPham",
                table: "ChiTietGioHangs",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMuc_Ma_DanhMucCha",
                table: "DanhMuc",
                column: "Ma_DanhMucCha");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_KhachHangMa_KhachHang",
                table: "DonHangs",
                column: "KhachHangMa_KhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_PhuongThucThanhToanMa_PhuongThuc",
                table: "DonHangs",
                column: "PhuongThucThanhToanMa_PhuongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_VoucherMa_Voucher",
                table: "DonHangs",
                column: "VoucherMa_Voucher");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_KhachHangMa_KhachHang",
                table: "GioHangs",
                column: "KhachHangMa_KhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPham_Ma_SanPham",
                table: "HinhAnhSanPham",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Ma_TaiKhoan",
                table: "KhachHang",
                column: "Ma_TaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_Ma_TaiKhoan",
                table: "NhanVien",
                column: "Ma_TaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_Ma_DanhMuc",
                table: "SanPham",
                column: "Ma_DanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_Ma_VaiTro",
                table: "TaiKhoan",
                column: "Ma_VaiTro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietGioHangs");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPham");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "DanhMuc");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "VaiTro");
        }
    }
}
