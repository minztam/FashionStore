using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class addData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001A",
                column: "Ten_DanhMuc",
                value: "Áo thun & Polo");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001B",
                column: "Ten_DanhMuc",
                value: "Áo sơ mi nam");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002A",
                column: "Ten_DanhMuc",
                value: "Áo nữ & Croptop");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002B",
                column: "Ten_DanhMuc",
                value: "Váy & Đầm nữ");

            migrationBuilder.InsertData(
                table: "DanhMuc",
                columns: new[] { "Ma_DanhMuc", "Ma_DanhMucCha", "Ten_DanhMuc", "Trang_Thai" },
                values: new object[,]
                {
                    { "DM001C", "DM001", "Áo khoác & Hoodie nam", true },
                    { "DM001D", "DM001", "Quần jeans nam", true },
                    { "DM001E", "DM001", "Quần short & Jogger nam", true },
                    { "DM002C", "DM002", "Quần nữ & Legging", true },
                    { "DM002D", "DM002", "Áo khoác nữ", true }
                });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP001",
                columns: new[] { "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "Form rộng chuẩn street style, vải dày dặn không xù lông, mặc cực mát", "Áo Thun Trơn Basic Unisex 100% Cotton 4 Chiều" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP002",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM001A", "Vải Pima cotton siêu mềm, cổ bẻ đứng form, logo thêu nổi bật", "Áo Polo Nam Cao Cấp Pima Cotton Form Chuẩn" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP003",
                columns: new[] { "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "Vải cotton thoáng mát, form chuẩn công sở, không nhăn", "Sơ Mi Nam Oxford Dài Tay Cao Cấp Form Chuẩn" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP004",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM001C", "Nỉ chân cua dày dặn, ấm áp mùa đông, form rộng cực chất", "Áo Hoodie Unisex Form Rộng Nỉ Bông 380GSM" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP005",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM001D", "Jeans cao cấp co giãn 4 chiều, form ôm tôn dáng, bền đẹp", "Quần Jeans Nam Slimfit Co Giãn 4 Chiều" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP006",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM001E", "Hot trend 2025, nhiều túi tiện lợi, chất kaki dày dặn", "Quần Short Kaki Nam Ống Rộng Cargo 6 Túi" });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "Ma_SanPham", "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham", "Trang_Thai" },
                values: new object[,]
                {
                    { "SP007", "DM002A", "Oversize cực xinh, mix đồ nào cũng đẹp, vải cotton mềm mại", "Áo Croptop Nữ Tay Lỡ Form Rộng Cotton", true },
                    { "SP008", "DM002B", "Che bụng mỡ cực tốt, mặc đi làm đi chơi đều hợp, freesize", "Váy Đầm Nữ Dáng A Babydoll Xinh Xắn", true },
                    { "SP022", "DM002A", "Mặc đi làm đi chơi đều sang chảnh, chất voan lụa mát", "Áo Sơ Mi Nữ Form Rộng Voan Lụa Cao Cấp", true }
                });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 189000m, 0m, "https://dosi-in.com/file/detailed/42/CDL10_1.jpg?w=670&h=670&fit=fill&fm=webp", "Đen", 20, 620 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 189000m, 0m, "https://dosi-in.com/file/detailed/42/CDL10_3.jpg?w=1000&h=1000&fit=fill&fm=webp", "Đen", 20, 590 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 189000m, 0m, "https://lados.vn/wp-content/uploads/2025/10/1-kem-ld9233.jpg", "M", "Xám", 10, 460 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 189000m, 0m, "https://lados.vn/wp-content/uploads/2025/10/ao-thun-tron-tay-ngan-dep-ld9233.jpg", "L", "SP001", "Xám", 10, 430 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", "Trắng", 25, 350 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", "Trắng", 25, 320 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", "XL", "SP002", "Trắng", 25, 100 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_xanh_den_1_c691c6dc769d483e860b6a1805be1b6f.jpg", "M", "SP002", "Xanh", 30, 380 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_xanh_den_1_c691c6dc769d483e860b6a1805be1b6f.jpg", "L", "SP002", "Xanh", 30, 360 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", "M", "SP002", "Hồng", 0, 310 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", "L", "SP002", "Đen", 0, 290 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", "XL", "SP002", "Đen", 0, 10 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 299000m, 0m, "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", "2XL", "SP002", "Đen", 0, 10 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 359000m, 0m, "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", "M", "SP003", "Xanh", 35, 300 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 359000m, 0m, "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", "L", "SP003", "Xanh", 35, 300 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 359000m, 0m, "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", "XL", "SP003", "Xanh", 35, 300 });

            migrationBuilder.InsertData(
                table: "SanPhamBienThe",
                columns: new[] { "Id", "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong", "Trang_Thai" },
                values: new object[,]
                {
                    { 17, 359000m, 0m, "https://lados.vn/wp-content/uploads/2024/07/1-den-8024.jpg", "M", "SP003", "Đen", 20, 260, true },
                    { 18, 359000m, 0m, "https://lados.vn/wp-content/uploads/2024/07/1-den-8024.jpg", "L", "SP003", "Đen", 20, 260, true },
                    { 19, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", "S", "SP004", "Đỏ đô", 30, 720, true },
                    { 20, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", "M", "SP004", "Đỏ đô", 30, 720, true },
                    { 21, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", "L", "SP004", "Đỏ đô", 30, 720, true },
                    { 22, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", "XL", "SP004", "Đỏ đô", 30, 720, true },
                    { 23, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", "S", "SP004", "Đen", 15, 550, true },
                    { 24, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", "M", "SP004", "Đen", 15, 550, true },
                    { 25, 429000m, 0m, "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", "L", "SP004", "Đen", 15, 550, true },
                    { 26, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", "M", "SP005", "Xanh nhạt", 28, 420, true },
                    { 27, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", "L", "SP005", "Xanh nhạt", 28, 420, true },
                    { 28, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", "XL", "SP005", "Xanh nhạt", 28, 420, true },
                    { 29, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/1-XANHDAM-LD4142.jpg", "L", "SP005", "Xanh Đậm", 28, 380, true },
                    { 30, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/4-DEN-LD4142.jpg", "M", "SP005", "Đen", 20, 350, true },
                    { 31, 489000m, 0m, "https://lados.vn/wp-content/uploads/2024/12/4-DEN-LD4142.jpg", "L", "SP005", "Đen", 20, 350, true },
                    { 32, 279000m, 0m, "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", "S", "SP006", "Đen", 25, 520, true },
                    { 33, 279000m, 0m, "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", "M", "SP006", "Đen", 25, 520, true },
                    { 34, 279000m, 0m, "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", "L", "SP006", "Đen", 25, 520, true },
                    { 35, 279000m, 0m, "https://lados.vn/wp-content/uploads/2025/05/2-KEM-LD4176.jpg", "M", "SP006", "Kem", 30, 460, true },
                    { 36, 279000m, 0m, "https://lados.vn/wp-content/uploads/2025/05/2-KEM-LD4176.jpg", "L", "SP006", "Kem", 30, 460, true }
                });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "Ma_SanPham", "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham", "Trang_Thai" },
                values: new object[,]
                {
                    { "SP009", "DM002C", "Vải cotton dày dặn, nâng mông tự nhiên, tôn dáng cực đẹp", "Quần Legging Nữ Lưng Cao Độn Mông", true },
                    { "SP010", "DM002D", "Form rộng đẹp, đi mưa nhẹ thoải mái, chống gió lạnh tốt", "Áo Khoác Gió Nữ 2 Lớp Chống Nước Có Mũ", true },
                    { "SP023", "DM001E", "Mặc nhà hoặc thể thao đều thoải mái, form đẹp", "Quần Jogger Nam Thun Bo Gấu Co Giãn", true }
                });

            migrationBuilder.InsertData(
                table: "SanPhamBienThe",
                columns: new[] { "Id", "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong", "Trang_Thai" },
                values: new object[,]
                {
                    { 49, 159000m, 0m, "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-3.jpg?v=1627743163683", "S", "SP007", "Vàng", 30, 680, true },
                    { 50, 159000m, 0m, "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-3.jpg?v=1627743163683", "M", "SP007", "Vàng", 30, 680, true },
                    { 51, 159000m, 0m, "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-4.jpg?v=1627743163683", "S", "SP007", "Xanh", 30, 680, true },
                    { 52, 349000m, 0m, "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", "S", "SP022", "Hồng", 30, 480, true },
                    { 53, 349000m, 0m, "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", "M", "SP022", "Hồng", 30, 480, true },
                    { 54, 349000m, 0m, "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", "L", "SP022", "Hồng", 30, 480, true },
                    { 55, 349000m, 0m, "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-kem-1_35fc1acf16f34bb9b2e141488e05a6e7_master.jpg", "S", "SP022", "Kem", 25, 420, true },
                    { 56, 349000m, 0m, "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-kem-1_35fc1acf16f34bb9b2e141488e05a6e7_master.jpg", "M", "SP022", "Kem", 25, 420, true },
                    { 57, 399000m, 0m, "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", "S", "SP008", "Cam", 30, 580, true },
                    { 58, 399000m, 0m, "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", "M", "SP008", "Cam", 30, 580, true },
                    { 59, 399000m, 0m, "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", "L", "SP008", "Cam", 30, 580, true },
                    { 37, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", "S", "SP023", "Xám đậm", 25, 620, true },
                    { 38, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", "M", "SP023", "Xám đậm", 25, 620, true },
                    { 39, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", "L", "SP023", "Xám đậm", 25, 620, true },
                    { 40, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", "XL", "SP023", "Xám đậm", 25, 620, true },
                    { 41, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", "S", "SP023", "Đen", 25, 590, true },
                    { 42, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", "M", "SP023", "Đen", 25, 590, true },
                    { 43, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", "L", "SP023", "Đen", 25, 590, true },
                    { 44, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", "M", "SP023", "Xanh rêu", 20, 550, true },
                    { 45, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", "L", "SP023", "Xanh rêu", 20, 550, true },
                    { 46, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", "XL", "SP023", "Xanh rêu", 20, 550, true },
                    { 47, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_xam_mon_1_af3a5dc03b574c6a981694529db928e1.jpg", "M", "SP023", "Xám", 20, 520, true },
                    { 48, 329000m, 0m, "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_xam_mon_1_af3a5dc03b574c6a981694529db928e1.jpg", "XL", "SP023", "Xám", 15, 480, true },
                    { 60, 229000m, 0m, "https://product.hstatic.net/200000900543/product/4062049265679_7a27ad55fa3a47fabe3d8077fd16d555_master.jpg", "M", "SP009", "Đen", 35, 820, true },
                    { 61, 459000m, 0m, "http://www.wetrek.vn/pic/products/ao-khoacd-gio-2-ldaop-gothiar-2l-ja_63844192522421_638979609463833850.jpg", "M", "SP010", "Đen", 30, 620, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001C");

            migrationBuilder.DeleteData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001D");

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP007");

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP008");

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP009");

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP010");

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP022");

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP023");

            migrationBuilder.DeleteData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001E");

            migrationBuilder.DeleteData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002C");

            migrationBuilder.DeleteData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002D");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001A",
                column: "Ten_DanhMuc",
                value: "Áo nam");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM001B",
                column: "Ten_DanhMuc",
                value: "Quần nam");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002A",
                column: "Ten_DanhMuc",
                value: "Áo nữ");

            migrationBuilder.UpdateData(
                table: "DanhMuc",
                keyColumn: "Ma_DanhMuc",
                keyValue: "DM002B",
                column: "Ten_DanhMuc",
                value: "Váy nữ");

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP001",
                columns: new[] { "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "Chất vải Oxford nhập khẩu, thoáng mát, không nhăn, form dáng chuẩn công sở và dự tiệc.", "Áo Sơ Mi Oxford Nam Cao Cấp" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP002",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM002B", "Lụa cao cấp mát mẻ, họa tiết hoa nhí vintage, dáng dài thướt tha, đi biển hay dự tiệc đều đẹp.", "Váy Maxi Lụa Hoa Nhí Thanh Lịch" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP003",
                columns: new[] { "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "Jeans co giãn 4 chiều, form slimfit tôn dáng, rách gối phong cách streetwear trẻ trung.", "Quần Jeans Nam Slimfit Rách Gối" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP004",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM002A", "Len mỏng nhẹ, dáng croptop tay dài, phối đồ mùa thu đông cực xinh, freesize từ 45-60kg.", "Áo Len Croptop Nữ Dáng Rộng" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP005",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM002A", "Sơ mi kẻ caro hàn quốc, form rộng oversize, mặc mát cả mùa hè.", "Sơ Mi Kẻ Sọc Nữ Form Rộng" });

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "Ma_SanPham",
                keyValue: "SP006",
                columns: new[] { "Ma_DanhMuc", "Mo_Ta", "Ten_SanPham" },
                values: new object[] { "DM001A", "Kaki dày dặn, có mũ tháo rời, chống nắng chống mưa nhẹ, form dáng trẻ trung.", "Áo Khoác Kaki Nam Có Mũ" });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 450000m, 399000m, "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800", "Trắng", 11, 80 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 450000m, 399000m, "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=800", "Trắng", 11, 100 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 470000m, 420000m, "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800", "XL", "Xanh Navy", 11, 60 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 780000m, null, "https://images.unsplash.com/photo-1594736797933-d0501ba2fe65?w=800", "S", "SP002", "Vàng Nhạt", null, 40 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 780000m, null, "https://images.unsplash.com/photo-1563170351-be82bc888aa4?w=800", "Vàng Nhạt", null, 55 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 780000m, 699000m, "https://images.unsplash.com/photo-1550616541-96cf6efa3c5e?w=800", "Xanh Mint", 10, 35 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 590000m, 499000m, "https://images.unsplash.com/photo-1542272604-787c3835535d?w=800", "29", "SP003", "Đen", 15, 70 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 590000m, 499000m, "https://images.unsplash.com/photo-1602293589930-45aad59ba3e4?w=800", "30", "SP003", "Đen", null, 90 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 610000m, null, "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=800", "31", "SP003", "Xanh Đậm", null, 50 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 350000m, 299000m, "https://images.unsplash.com/photo-1611318440154-8f60a0b9f512?w=800", "Freesize", "SP004", "Be", 15, 120 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 350000m, null, "https://images.unsplash.com/photo-1617019114583-affb34d1b3cd?w=800", "Freesize", "SP004", "Đỏ Rượu", null, 80 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 380000m, 329000m, "https://images.unsplash.com/photo-1598550874175-4d0ef436c909?w=800", "Freesize", "SP005", "Trắng Kẻ Xanh", 13, 100 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 690000m, 590000m, "https://images.unsplash.com/photo-1551029506-0807df4e2031?w=800", "M", "SP006", "Xanh Rêu", 14, 60 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 690000m, 590000m, "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=800", "L", "SP006", "Xanh Rêu", null, 80 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 710000m, null, "https://images.unsplash.com/photo-1552374196-c4e7ffc6e126?w=800", "XL", "SP006", "Đen", null, 45 });

            migrationBuilder.UpdateData(
                table: "SanPhamBienThe",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Gia_BienThe", "Gia_Giam", "HinhAnh", "Kich_Thuoc", "Ma_SanPham", "Mau_Sac", "PhanTramGiam", "So_Luong" },
                values: new object[] { 690000m, null, "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800", "M", "SP006", "Be", null, 55 });
        }
    }
}
