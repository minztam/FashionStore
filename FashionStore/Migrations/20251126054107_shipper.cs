using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class shipper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ma_Shipper",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    Ma_Shipper = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ma_TaiKhoan = table.Column<int>(type: "int", nullable: false),
                    Ten_DayDu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BienSoXe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippers", x => x.Ma_Shipper);
                    table.ForeignKey(
                        name: "FK_Shippers_TaiKhoan_Ma_TaiKhoan",
                        column: x => x.Ma_TaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "Ma_TaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "VaiTro",
                columns: new[] { "Ma_VaiTro", "Ten_VaiTro" },
                values: new object[] { "3333-3333-3333-3333", "Shipper" });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_Ma_Shipper",
                table: "DonHang",
                column: "Ma_Shipper");

            migrationBuilder.CreateIndex(
                name: "IX_Shippers_Ma_TaiKhoan",
                table: "Shippers",
                column: "Ma_TaiKhoan",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_Shippers_Ma_Shipper",
                table: "DonHang",
                column: "Ma_Shipper",
                principalTable: "Shippers",
                principalColumn: "Ma_Shipper",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_Shippers_Ma_Shipper",
                table: "DonHang");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_Ma_Shipper",
                table: "DonHang");

            migrationBuilder.DeleteData(
                table: "VaiTro",
                keyColumn: "Ma_VaiTro",
                keyValue: "3333-3333-3333-3333");

            migrationBuilder.DropColumn(
                name: "Ma_Shipper",
                table: "DonHang");
        }
    }
}
