using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class addNewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "Ma_TaiKhoan", "Da_XacThuc", "Email", "Han_XacThuc", "Ma_VaiTro", "Ma_XacThuc", "Mat_Khau", "Ngay_Tao", "Ten_DangNhap", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, true, "admin@gmail.com", new DateTime(2025, 12, 3, 10, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3963), "1111-1111-1111-1111", "9pug2MZlj0m6HZPJ6Njolg==", "string", new DateTime(2025, 12, 2, 17, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3937), "admin", true },
                    { 2, true, "khachhang@gmail.com", new DateTime(2025, 12, 3, 10, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3963), "2222-2222-2222-2222", "9pug2MZlj0m6HZPJ6Njolg==", "string", new DateTime(2025, 12, 2, 17, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3937), "khachhang", true },
                    { 3, true, "staff@gmail.com", new DateTime(2025, 12, 3, 10, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3963), "1111-2222-1111-2222", "9pug2MZlj0m6HZPJ6Njolg==", "string", new DateTime(2025, 12, 2, 17, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3937), "staff", true },
                    { 4, true, "shipper@gmail.com", new DateTime(2025, 12, 3, 10, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3963), "3333-3333-3333-3333", "9pug2MZlj0m6HZPJ6Njolg==", "string", new DateTime(2025, 12, 2, 17, 30, 42, 261, DateTimeKind.Unspecified).AddTicks(3937), "shipper", true }
                });

            migrationBuilder.InsertData(
                table: "KhachHang",
                columns: new[] { "Ma_KhachHang", "DiaChi", "Hinh_Anh", "HoTen", "Ma_TaiKhoan", "SoDienThoai" },
                values: new object[] { 1, "Thanh Hóa", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSk8nOS9W4aP2bq6MY4K-HlIusxbKz4Bw1PEw&s", "Trịnh Anh Đức", 2, "0989105436" });

            migrationBuilder.InsertData(
                table: "NhanVien",
                columns: new[] { "Ma_NhanVien", "DiaChi", "Hinh_Anh", "HoTen", "Ma_TaiKhoan", "SoDienThoai" },
                values: new object[] { 2, "Sài Gòn", "https://cdn.24h.com.vn/upload/2-2021/images/2021-04-09/untitled-8-1617960812-512-width650height900.jpg", "Ngô Đình Văn", 3, "0982343544" });

            migrationBuilder.InsertData(
                table: "Shippers",
                columns: new[] { "Ma_Shipper", "BienSoXe", "HinhAnh", "Ma_TaiKhoan", "SoDienThoai", "Ten_DayDu", "TrangThai" },
                values: new object[] { 1, "69N4-9999", "https://cdnphoto.dantri.com.vn/PZJhMVnXWQcyRwL7GOzw539WApw=/thumb_w/1020/2024/02/01/ghtk-crop-edited-1706744665204.jpeg", 4, "0982343222", "Trương Minh Tâm", "online" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KhachHang",
                keyColumn: "Ma_KhachHang",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NhanVien",
                keyColumn: "Ma_NhanVien",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shippers",
                keyColumn: "Ma_Shipper",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "Ma_TaiKhoan",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "Ma_TaiKhoan",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "Ma_TaiKhoan",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "Ma_TaiKhoan",
                keyValue: 4);
        }
    }
}
