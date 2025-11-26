using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class xoahinhanh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhSanPham_SanPham_Ma_SanPham",
                table: "HinhAnhSanPham");

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "HinhAnhSanPham",
                keyColumn: "Ma_HinhAnh",
                keyValue: 21);

            migrationBuilder.AddColumn<bool>(
                name: "Trang_Thai",
                table: "SanPhamBienThe",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 1,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 2,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 3,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 4,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 5,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 6,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 7,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 8,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 9,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 10,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 11,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 12,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 13,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 14,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 15,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 16,
                column: "Trang_Thai",
                value: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhSanPham_SanPham_Ma_SanPham",
                table: "HinhAnhSanPham",
                column: "Ma_SanPham",
                principalTable: "SanPham",
                principalColumn: "Ma_SanPham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhSanPham_SanPham_Ma_SanPham",
                table: "HinhAnhSanPham");

            migrationBuilder.DropColumn(
                name: "Trang_Thai",
                table: "SanPhamBienThe");

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

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhSanPham_SanPham_Ma_SanPham",
                table: "HinhAnhSanPham",
                column: "Ma_SanPham",
                principalTable: "SanPham",
                principalColumn: "Ma_SanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
