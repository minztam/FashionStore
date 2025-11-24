using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class fixchitietdonhang2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PhuongThucThanhToans",
                columns: new[] { "Ma_PhuongThuc", "Ten_PhuongThuc" },
                values: new object[,]
                {
                    { 1, "Thanh toán khi nhận hàng (COD)" },
                    { 2, "Ví điện tử" },
                    { 3, "Chuyển khoản ngân hàng" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhuongThucThanhToans",
                keyColumn: "Ma_PhuongThuc",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PhuongThucThanhToans",
                keyColumn: "Ma_PhuongThuc",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PhuongThucThanhToans",
                keyColumn: "Ma_PhuongThuc",
                keyValue: 3);
        }
    }
}
