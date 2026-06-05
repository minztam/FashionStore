using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class sanphambienthe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "SanPhamBienThe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 1,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 2,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 3,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 4,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1594736797933-d0501ba2fe65?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 5,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1563170351-be82bc888aa4?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 6,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1550616541-96cf6efa3c5e?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 7,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1542272604-787c3835535d?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 8,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1602293589930-45aad59ba3e4?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 9,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 10,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1611318440154-8f60a0b9f512?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 11,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1617019114583-affb34d1b3cd?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 12,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1598550874175-4d0ef436c909?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 13,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1551029506-0807df4e2031?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 14,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 15,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1552374196-c4e7ffc6e126?w=800");

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 16,
                column: "HinhAnh",
                value: "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "SanPhamBienThe");
        }
    }
}
