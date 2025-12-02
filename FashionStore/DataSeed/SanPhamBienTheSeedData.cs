using FashionStore.Models;
using System.Collections.Generic;

namespace FashionStore.DataSeed
{
    public static class SanPhamBienTheSeedData
    {
        public static IEnumerable<SanPhamBienThe> GetSeedData()
        {
            return new List<SanPhamBienThe>
            {
                // SP001 - Áo thun basic unisex            
                new SanPhamBienThe { Id = 1,  Ma_SanPham = "SP001", Mau_Sac = "Đen",   Kich_Thuoc = "M",  So_Luong = 620, Gia_BienThe = 189000, PhanTramGiam = 20, HinhAnh = "https://dosi-in.com/file/detailed/42/CDL10_1.jpg?w=670&h=670&fit=fill&fm=webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 2,  Ma_SanPham = "SP001", Mau_Sac = "Đen",   Kich_Thuoc = "L",  So_Luong = 590, Gia_BienThe = 189000, PhanTramGiam = 20, HinhAnh = "https://dosi-in.com/file/detailed/42/CDL10_3.jpg?w=1000&h=1000&fit=fill&fm=webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 3,  Ma_SanPham = "SP001", Mau_Sac = "Xám",   Kich_Thuoc = "M",  So_Luong = 460, Gia_BienThe = 189000, PhanTramGiam = 10, HinhAnh = "https://lados.vn/wp-content/uploads/2025/10/1-kem-ld9233.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 4,  Ma_SanPham = "SP001", Mau_Sac = "Xám",   Kich_Thuoc = "L",  So_Luong = 430, Gia_BienThe = 189000, PhanTramGiam = 10, HinhAnh = "https://lados.vn/wp-content/uploads/2025/10/ao-thun-tron-tay-ngan-dep-ld9233.jpg", Trang_Thai = true },

                // SP002 - Áo Polo nam
                new SanPhamBienThe { Id = 5,  Ma_SanPham = "SP002", Mau_Sac = "Trắng", Kich_Thuoc = "M", So_Luong = 350, Gia_BienThe = 299000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 6,  Ma_SanPham = "SP002", Mau_Sac = "Trắng", Kich_Thuoc = "L", So_Luong = 320, Gia_BienThe = 299000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 7,  Ma_SanPham = "SP002", Mau_Sac = "Trắng", Kich_Thuoc = "XL", So_Luong = 100, Gia_BienThe = 299000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_trang_1_a24816452efe4226bdd5fee6eb8375ec.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 8,  Ma_SanPham = "SP002", Mau_Sac = "Xanh",  Kich_Thuoc = "M", So_Luong = 380, Gia_BienThe = 299000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_xanh_den_1_c691c6dc769d483e860b6a1805be1b6f.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 9, Ma_SanPham = "SP002", Mau_Sac = "Xanh",  Kich_Thuoc = "L", So_Luong = 360, Gia_BienThe = 299000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_xanh_den_1_c691c6dc769d483e860b6a1805be1b6f.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 10, Ma_SanPham = "SP002", Mau_Sac = "Hồng",   Kich_Thuoc = "M", So_Luong = 310, Gia_BienThe = 299000, PhanTramGiam = 0,  HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 11, Ma_SanPham = "SP002", Mau_Sac = "Đen",   Kich_Thuoc = "L", So_Luong = 290, Gia_BienThe = 299000, PhanTramGiam = 0,  HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 12, Ma_SanPham = "SP002", Mau_Sac = "Đen",   Kich_Thuoc = "XL", So_Luong = 10, Gia_BienThe = 299000, PhanTramGiam = 0,  HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 13, Ma_SanPham = "SP002", Mau_Sac = "Đen",   Kich_Thuoc = "2XL", So_Luong = 10, Gia_BienThe = 299000, PhanTramGiam = 0,  HinhAnh = "https://product.hstatic.net/1000369857/product/ao_ab19_hong_1_5ef748bd794749b7b0eb66c8bf6d3e33.jpg", Trang_Thai = true },

                // SP003 - Sơ mi nam oxford
                new SanPhamBienThe { Id = 14, Ma_SanPham = "SP003", Mau_Sac = "Xanh",     Kich_Thuoc = "M", So_Luong = 300, Gia_BienThe = 359000, PhanTramGiam = 35, HinhAnh = "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 15, Ma_SanPham = "SP003", Mau_Sac = "Xanh",     Kich_Thuoc = "L", So_Luong = 300, Gia_BienThe = 359000, PhanTramGiam = 35, HinhAnh = "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 16, Ma_SanPham = "SP003", Mau_Sac = "Xanh",     Kich_Thuoc = "XL", So_Luong = 300, Gia_BienThe = 359000, PhanTramGiam = 35, HinhAnh = "https://lados.vn/wp-content/uploads/2024/07/4-xanhnhat-8024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 17, Ma_SanPham = "SP003", Mau_Sac = "Đen", Kich_Thuoc = "M", So_Luong = 260, Gia_BienThe = 359000, PhanTramGiam = 20, HinhAnh = "https://lados.vn/wp-content/uploads/2024/07/1-den-8024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 18, Ma_SanPham = "SP003", Mau_Sac = "Đen", Kich_Thuoc = "L", So_Luong = 260, Gia_BienThe = 359000, PhanTramGiam = 20, HinhAnh = "https://lados.vn/wp-content/uploads/2024/07/1-den-8024.jpg", Trang_Thai = true },

                // SP004 - Hoodie unisex
                new SanPhamBienThe { Id = 19, Ma_SanPham = "SP004", Mau_Sac = "Đỏ đô", Kich_Thuoc = "S", So_Luong = 720, Gia_BienThe = 429000, PhanTramGiam = 30, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 20, Ma_SanPham = "SP004", Mau_Sac = "Đỏ đô", Kich_Thuoc = "M", So_Luong = 720, Gia_BienThe = 429000, PhanTramGiam = 30, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 21, Ma_SanPham = "SP004", Mau_Sac = "Đỏ đô", Kich_Thuoc = "L", So_Luong = 720, Gia_BienThe = 429000, PhanTramGiam = 30, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 22, Ma_SanPham = "SP004", Mau_Sac = "Đỏ đô", Kich_Thuoc = "XL", So_Luong = 720, Gia_BienThe = 429000, PhanTramGiam = 30, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-bitter-choco-ao-ni-unisex-1-jpg-omzo.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 23, Ma_SanPham = "SP004", Mau_Sac = "Đen",  Kich_Thuoc = "S", So_Luong = 550, Gia_BienThe = 429000, PhanTramGiam = 15, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 24, Ma_SanPham = "SP004", Mau_Sac = "Đen",  Kich_Thuoc = "M", So_Luong = 550, Gia_BienThe = 429000, PhanTramGiam = 15, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", Trang_Thai = true },
                new SanPhamBienThe { Id = 25, Ma_SanPham = "SP004", Mau_Sac = "Đen",  Kich_Thuoc = "L", So_Luong = 550, Gia_BienThe = 429000, PhanTramGiam = 15, HinhAnh = "https://media.routine.vn/1987x0/prod/media/10f24hodu001-black-1-ao-ni-unisex-jpg-t2kt.webp", Trang_Thai = true },

                // SP005 - Jeans nam
                new SanPhamBienThe { Id = 26, Ma_SanPham = "SP005", Mau_Sac = "Xanh nhạt", Kich_Thuoc = "M", So_Luong = 420, Gia_BienThe = 489000, PhanTramGiam = 28, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 27, Ma_SanPham = "SP005", Mau_Sac = "Xanh nhạt", Kich_Thuoc = "L", So_Luong = 420, Gia_BienThe = 489000, PhanTramGiam = 28, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 28, Ma_SanPham = "SP005", Mau_Sac = "Xanh nhạt", Kich_Thuoc = "XL", So_Luong = 420, Gia_BienThe = 489000, PhanTramGiam = 28, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/2-XANHNHAT-LD4142.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 29, Ma_SanPham = "SP005", Mau_Sac = "Xanh Đậm", Kich_Thuoc = "L", So_Luong = 380, Gia_BienThe = 489000, PhanTramGiam = 28, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/1-XANHDAM-LD4142.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 30, Ma_SanPham = "SP005", Mau_Sac = "Đen", Kich_Thuoc = "M", So_Luong = 350, Gia_BienThe = 489000, PhanTramGiam = 20, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/4-DEN-LD4142.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 31, Ma_SanPham = "SP005", Mau_Sac = "Đen", Kich_Thuoc = "L", So_Luong = 350, Gia_BienThe = 489000, PhanTramGiam = 20, HinhAnh = "https://lados.vn/wp-content/uploads/2024/12/4-DEN-LD4142.jpg", Trang_Thai = true },

                // SP006 - Short kaki cargo
                new SanPhamBienThe { Id = 32, Ma_SanPham = "SP006", Mau_Sac = "Đen",   Kich_Thuoc = "S", So_Luong = 520, Gia_BienThe = 279000, PhanTramGiam = 25, HinhAnh = "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 33, Ma_SanPham = "SP006", Mau_Sac = "Đen",  Kich_Thuoc = "M", So_Luong = 520, Gia_BienThe = 279000, PhanTramGiam = 25, HinhAnh = "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 34, Ma_SanPham = "SP006", Mau_Sac = "Đen",   Kich_Thuoc = "L", So_Luong = 520, Gia_BienThe = 279000, PhanTramGiam = 25, HinhAnh = "https://lados.vn/wp-content/uploads/2025/05/1-DEN-LD4176.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 35, Ma_SanPham = "SP006", Mau_Sac = "Kem",  Kich_Thuoc = "M", So_Luong = 460, Gia_BienThe = 279000, PhanTramGiam = 30, HinhAnh = "https://lados.vn/wp-content/uploads/2025/05/2-KEM-LD4176.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 36, Ma_SanPham = "SP006", Mau_Sac = "Kem",  Kich_Thuoc = "L", So_Luong = 460, Gia_BienThe = 279000, PhanTramGiam = 30, HinhAnh = "https://lados.vn/wp-content/uploads/2025/05/2-KEM-LD4176.jpg", Trang_Thai = true },

                // SP023 - Jogger nam
                new SanPhamBienThe { Id = 37, Ma_SanPham = "SP023", Mau_Sac = "Xám đậm", Kich_Thuoc = "S",  So_Luong = 620, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 38, Ma_SanPham = "SP023", Mau_Sac = "Xám đậm", Kich_Thuoc = "M",  So_Luong = 620, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 39, Ma_SanPham = "SP023", Mau_Sac = "Xám đậm", Kich_Thuoc = "L",  So_Luong = 620, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 40, Ma_SanPham = "SP023", Mau_Sac = "Xám đậm", Kich_Thuoc = "XL",  So_Luong = 620, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_28_582ad8f1eee945228c9b75aa515756bb.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 41, Ma_SanPham = "SP023", Mau_Sac = "Đen", Kich_Thuoc = "S",  So_Luong = 590, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 42, Ma_SanPham = "SP023", Mau_Sac = "Đen", Kich_Thuoc = "M",  So_Luong = 590, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 43, Ma_SanPham = "SP023", Mau_Sac = "Đen", Kich_Thuoc = "L",  So_Luong = 590, Gia_BienThe = 329000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_25_54d21d9a57404e4fb00ffb09c2c241f1.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 44, Ma_SanPham = "SP023", Mau_Sac = "Xanh rêu", Kich_Thuoc = "M",  So_Luong = 550, Gia_BienThe = 329000, PhanTramGiam = 20, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 45, Ma_SanPham = "SP023", Mau_Sac = "Xanh rêu", Kich_Thuoc = "L",  So_Luong = 550, Gia_BienThe = 329000, PhanTramGiam = 20, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 46, Ma_SanPham = "SP023", Mau_Sac = "Xanh rêu", Kich_Thuoc = "XL",  So_Luong = 550, Gia_BienThe = 329000, PhanTramGiam = 20, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_1200x1200_0000_layer_26_b22be9b19a4f4121827c866c78b45394.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 47, Ma_SanPham = "SP023", Mau_Sac = "Xám", Kich_Thuoc = "M",  So_Luong = 520, Gia_BienThe = 329000, PhanTramGiam = 20, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_xam_mon_1_af3a5dc03b574c6a981694529db928e1.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 48, Ma_SanPham = "SP023", Mau_Sac = "Xám", Kich_Thuoc = "XL", So_Luong = 480, Gia_BienThe = 329000, PhanTramGiam = 15, HinhAnh = "https://product.hstatic.net/1000369857/product/kaki_dai_jogger_xam_mon_1_af3a5dc03b574c6a981694529db928e1.jpg", Trang_Thai = true },

                // SP007 - Croptop nữ
                new SanPhamBienThe { Id = 49, Ma_SanPham = "SP007", Mau_Sac = "Vàng", Kich_Thuoc = "S",  So_Luong = 680, Gia_BienThe = 159000, PhanTramGiam = 30, HinhAnh = "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-3.jpg?v=1627743163683", Trang_Thai = true },
                new SanPhamBienThe { Id = 50, Ma_SanPham = "SP007", Mau_Sac = "Vàng", Kich_Thuoc = "M",  So_Luong = 680, Gia_BienThe = 159000, PhanTramGiam = 30, HinhAnh = "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-3.jpg?v=1627743163683", Trang_Thai = true },
                new SanPhamBienThe { Id = 51, Ma_SanPham = "SP007", Mau_Sac = "Xanh", Kich_Thuoc = "S",  So_Luong = 680, Gia_BienThe = 159000, PhanTramGiam = 30, HinhAnh = "https://bizweb.dktcdn.net/100/287/440/products/ao-croptop-om-eo-nu-local-brand-davies-4.jpg?v=1627743163683", Trang_Thai = true },
               
                // SP022 - Áo sơ mi nữ
                new SanPhamBienThe { Id = 52, Ma_SanPham = "SP022", Mau_Sac = "Hồng", Kich_Thuoc = "S",  So_Luong = 480, Gia_BienThe = 349000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 53, Ma_SanPham = "SP022", Mau_Sac = "Hồng", Kich_Thuoc = "M",  So_Luong = 480, Gia_BienThe = 349000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 54, Ma_SanPham = "SP022", Mau_Sac = "Hồng", Kich_Thuoc = "L",  So_Luong = 480, Gia_BienThe = 349000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-hong-1_f5a75811dced4e26868a7d23bfb9dc1e_master.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 55, Ma_SanPham = "SP022", Mau_Sac = "Kem", Kich_Thuoc = "S", So_Luong = 420, Gia_BienThe = 349000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-kem-1_35fc1acf16f34bb9b2e141488e05a6e7_master.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 56, Ma_SanPham = "SP022", Mau_Sac = "Kem", Kich_Thuoc = "M", So_Luong = 420, Gia_BienThe = 349000, PhanTramGiam = 25, HinhAnh = "https://product.hstatic.net/200000588593/product/2sp23s051-ao-so-mi-kieu-nu-kem-1_35fc1acf16f34bb9b2e141488e05a6e7_master.jpg", Trang_Thai = true },
                
                // SP008 - Váy đầm nữ
                new SanPhamBienThe { Id = 57, Ma_SanPham = "SP008", Mau_Sac = "Cam",    Kich_Thuoc = "S", So_Luong = 580, Gia_BienThe = 399000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 58, Ma_SanPham = "SP008", Mau_Sac = "Cam",    Kich_Thuoc = "M", So_Luong = 580, Gia_BienThe = 399000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", Trang_Thai = true },
                new SanPhamBienThe { Id = 59, Ma_SanPham = "SP008", Mau_Sac = "Cam",    Kich_Thuoc = "L", So_Luong = 580, Gia_BienThe = 399000, PhanTramGiam = 30, HinhAnh = "https://product.hstatic.net/200000525243/product/image-cam-5-dam-kieu-linen-day-cheo-linen-nu-n-m-2005004_be45b0ff02764b16b9d60e373905cb16_1024x1024.jpg", Trang_Thai = true },
                
                // SP009 - Legging nữ
                new SanPhamBienThe { Id = 60, Ma_SanPham = "SP009", Mau_Sac = "Đen",  Kich_Thuoc = "M",  So_Luong = 820, Gia_BienThe = 229000, PhanTramGiam = 35, HinhAnh = "https://product.hstatic.net/200000900543/product/4062049265679_7a27ad55fa3a47fabe3d8077fd16d555_master.jpg", Trang_Thai = true },
                
                // SP010 - Áo khoác gió nữ
                new SanPhamBienThe { Id = 61, Ma_SanPham = "SP010", Mau_Sac = "Đen",   Kich_Thuoc = "M", So_Luong = 620, Gia_BienThe = 459000, PhanTramGiam = 30, HinhAnh = "http://www.wetrek.vn/pic/products/ao-khoacd-gio-2-ldaop-gothiar-2l-ja_63844192522421_638979609463833850.jpg", Trang_Thai = true }
            };
        }
    }
}