using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                        onDelete: ReferentialAction.Restrict);
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
                name: "SanPhamBienThe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mau_Sac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kich_Thuoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    Gia_BienThe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gia_Giam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhanTramGiam = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamBienThe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SanPhamBienThe_SanPham_Ma_SanPham",
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
                name: "DonHang",
                columns: table => new
                {
                    Ma_DonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false),
                    Ngay_Dat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tong_Tien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Trang_Thai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma_PhuongThuc = table.Column<int>(type: "int", nullable: false),
                    Ma_Voucher = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.Ma_DonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_Ma_KhachHang",
                        column: x => x.Ma_KhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Ma_KhachHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHang_PhuongThucThanhToans_Ma_PhuongThuc",
                        column: x => x.Ma_PhuongThuc,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "Ma_PhuongThuc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHang_Vouchers_Ma_Voucher",
                        column: x => x.Ma_Voucher,
                        principalTable: "Vouchers",
                        principalColumn: "Ma_Voucher");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    Ma_GioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_KhachHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.Ma_GioHang);
                    table.ForeignKey(
                        name: "FK_GioHang_KhachHang_Ma_KhachHang",
                        column: x => x.Ma_KhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Ma_KhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    Ma_DonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => new { x.Ma_DonHang, x.Ma_SanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_Ma_DonHang",
                        column: x => x.Ma_DonHang,
                        principalTable: "DonHang",
                        principalColumn: "Ma_DonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham_Ma_SanPham",
                        column: x => x.Ma_SanPham,
                        principalTable: "SanPham",
                        principalColumn: "Ma_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    Ma_GioHang = table.Column<int>(type: "int", nullable: false),
                    Ma_SanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => new { x.Ma_GioHang, x.Ma_SanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_GioHang_Ma_GioHang",
                        column: x => x.Ma_GioHang,
                        principalTable: "GioHang",
                        principalColumn: "Ma_GioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_SanPham_Ma_SanPham",
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
                columns: new[] { "Ma_SanPham", "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham", "Trang_Thai" },
                values: new object[,]
                {
                    { "SP001", "DM001A", "Chất vải Oxford nhập khẩu, thoáng mát, không nhăn, form dáng chuẩn công sở và dự tiệc.", "Áo Sơ Mi Oxford Nam Cao Cấp", true },
                    { "SP002", "DM002B", "Lụa cao cấp mát mẻ, họa tiết hoa nhí vintage, dáng dài thướt tha, đi biển hay dự tiệc đều đẹp.", "Váy Maxi Lụa Hoa Nhí Thanh Lịch", true },
                    { "SP003", "DM001B", "Jeans co giãn 4 chiều, form slimfit tôn dáng, rách gối phong cách streetwear trẻ trung.", "Quần Jeans Nam Slimfit Rách Gối", true },
                    { "SP004", "DM002A", "Len mỏng nhẹ, dáng croptop tay dài, phối đồ mùa thu đông cực xinh, freesize từ 45-60kg.", "Áo Len Croptop Nữ Dáng Rộng", true },
                    { "SP005", "DM002A", "Sơ mi kẻ caro hàn quốc, form rộng oversize, mặc mát cả mùa hè.", "Sơ Mi Kẻ Sọc Nữ Form Rộng", true },
                    { "SP006", "DM001A", "Kaki dày dặn, có mũ tháo rời, chống nắng chống mưa nhẹ, form dáng trẻ trung.", "Áo Khoác Kaki Nam Có Mũ", true }
                });

            migrationBuilder.InsertData(
                table: "HinhAnhSanPham",
                columns: new[] { "Ma_HinhAnh", "DuongDan", "Ma_SanPham" },
                values: new object[,]
                {
                    { 1, "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800", "SP001" },
                    { 2, "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=800", "SP001" },
                    { 3, "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800", "SP001" },
                    { 4, "https://images.unsplash.com/photo-1617114919317-8f6dce8e9df5?w=800", "SP001" },
                    { 5, "https://images.unsplash.com/photo-1594736797933-d0501ba2fe65?w=800", "SP002" },
                    { 6, "https://images.unsplash.com/photo-1563170351-be82bc888aa4?w=800", "SP002" },
                    { 7, "https://images.unsplash.com/photo-1550616541-96cf6efa3c5e?w=800", "SP002" },
                    { 8, "https://images.unsplash.com/photo-1585487000160-6ebcfceb0d03?w=800", "SP002" },
                    { 9, "https://images.unsplash.com/photo-1542272604-787c3835535d?w=800", "SP003" },
                    { 10, "https://images.unsplash.com/photo-1602293589930-45aad59ba3e4?w=800", "SP003" },
                    { 11, "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=800", "SP003" },
                    { 12, "https://images.unsplash.com/photo-1611318440154-8f60a0b9f512?w=800", "SP004" },
                    { 13, "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800", "SP004" },
                    { 14, "https://images.unsplash.com/photo-1617019114583-affb34d1b3cd?w=800", "SP004" },
                    { 15, "https://images.unsplash.com/photo-1598550874175-4d0ef436c909?w=800", "SP005" },
                    { 16, "https://images.unsplash.com/photo-1612902376491-7a8a9b42425f?w=800", "SP005" },
                    { 17, "https://images.unsplash.com/photo-1622473596033-9d8d1e7e6a5a?w=800", "SP005" },
                    { 18, "https://images.unsplash.com/photo-1551029506-0807df4e2031?w=800", "SP006" },
                    { 19, "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=800", "SP006" },
                    { 20, "https://images.unsplash.com/photo-1552374196-c4e7ffc6e126?w=800", "SP006" },
                    { 21, "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800", "SP006" }
                });

            migrationBuilder.InsertData(
                table: "SanPhamBienThe",
                columns: new[] { "Id", "Gia_BienThe", "Gia_Giam", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[,]
                {
                    { 1, 450000m, 399000m, "M", "SP001", "Trắng", 11, 80 },
                    { 2, 450000m, 399000m, "L", "SP001", "Trắng", 11, 100 },
                    { 3, 470000m, 420000m, "XL", "SP001", "Xanh Navy", 11, 60 },
                    { 4, 780000m, null, "S", "SP002", "Vàng Nhạt", null, 40 },
                    { 5, 780000m, null, "M", "SP002", "Vàng Nhạt", null, 55 },
                    { 6, 780000m, 699000m, "L", "SP002", "Xanh Mint", 10, 35 },
                    { 7, 590000m, 499000m, "29", "SP003", "Đen", 15, 70 },
                    { 8, 590000m, 499000m, "30", "SP003", "Đen", null, 90 },
                    { 9, 610000m, null, "31", "SP003", "Xanh Đậm", null, 50 },
                    { 10, 350000m, 299000m, "Freesize", "SP004", "Be", 15, 120 },
                    { 11, 350000m, null, "Freesize", "SP004", "Đỏ Rượu", null, 80 },
                    { 12, 380000m, 329000m, "Freesize", "SP005", "Trắng Kẻ Xanh", 13, 100 },
                    { 13, 690000m, 590000m, "M", "SP006", "Xanh Rêu", 14, 60 },
                    { 14, 690000m, 590000m, "L", "SP006", "Xanh Rêu", null, 80 },
                    { 15, 710000m, null, "XL", "SP006", "Đen", null, 45 },
                    { 16, 690000m, null, "M", "SP006", "Be", null, 55 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_Ma_SanPham",
                table: "ChiTietDonHang",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_Ma_SanPham",
                table: "ChiTietGioHang",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMuc_Ma_DanhMucCha",
                table: "DanhMuc",
                column: "Ma_DanhMucCha");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_KhachHang",
                table: "DonHang",
                column: "Ma_KhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_PhuongThuc",
                table: "DonHang",
                column: "Ma_PhuongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_Voucher",
                table: "DonHang",
                column: "Ma_Voucher");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_Ma_KhachHang",
                table: "GioHang",
                column: "Ma_KhachHang");

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
                name: "IX_SanPhamBienThe_Ma_SanPham",
                table: "SanPhamBienThe",
                column: "Ma_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_Ma_VaiTro",
                table: "TaiKhoan",
                column: "Ma_VaiTro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPham");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "SanPhamBienThe");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "GioHang");

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
