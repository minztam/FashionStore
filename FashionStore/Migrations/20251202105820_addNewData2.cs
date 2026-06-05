using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class addNewData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NhanVien",
                keyColumn: "Ma_NhanVien",
                keyValue: 2,
                column: "HoTen",
                value: "Nguyễn Đình Văn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NhanVien",
                keyColumn: "Ma_NhanVien",
                keyValue: 2,
                column: "HoTen",
                value: "Ngô Đình Văn");
        }
    }
}
