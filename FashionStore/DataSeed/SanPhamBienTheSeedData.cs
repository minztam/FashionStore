using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class SanPhamBienTheSeedData
    {
        public static IEnumerable<SanPhamBienThe> GetSeedData()
        {
            return new List<SanPhamBienThe>
            {
                // SP001 - Áo sơ mi nam
                new() { Id = 1,  Ma_SanPham = "SP001", Mau_Sac = "Trắng",     Kich_Thuoc = "M",   So_Luong = 80,  Gia_BienThe = 450000, Gia_Giam = 399000, PhanTramGiam = 11 },
                new() { Id = 2,  Ma_SanPham = "SP001", Mau_Sac = "Trắng",     Kich_Thuoc = "L",   So_Luong = 100, Gia_BienThe = 450000, Gia_Giam = 399000, PhanTramGiam = 11 },
                new() { Id = 3,  Ma_SanPham = "SP001", Mau_Sac = "Xanh Navy", Kich_Thuoc = "XL",  So_Luong = 60,  Gia_BienThe = 470000, Gia_Giam = 420000, PhanTramGiam = 11 },

                // SP002 - Váy maxi nữ
                new() { Id = 4,  Ma_SanPham = "SP002", Mau_Sac = "Vàng Nhạt", Kich_Thuoc = "S",   So_Luong = 40,  Gia_BienThe = 780000 },
                new() { Id = 5,  Ma_SanPham = "SP002", Mau_Sac = "Vàng Nhạt", Kich_Thuoc = "M",   So_Luong = 55,  Gia_BienThe = 780000 },
                new() { Id = 6,  Ma_SanPham = "SP002", Mau_Sac = "Xanh Mint", Kich_Thuoc = "L",   So_Luong = 35,  Gia_BienThe = 780000, Gia_Giam = 699000, PhanTramGiam = 10 },

                // SP003 - Quần jeans nam
                new() { Id = 7,  Ma_SanPham = "SP003", Mau_Sac = "Đen",       Kich_Thuoc = "29",  So_Luong = 70,  Gia_BienThe = 590000, Gia_Giam = 499000, PhanTramGiam = 15 },
                new() { Id = 8,  Ma_SanPham = "SP003", Mau_Sac = "Đen",       Kich_Thuoc = "30",  So_Luong = 90,  Gia_BienThe = 590000, Gia_Giam = 499000 },
                new() { Id = 9,  Ma_SanPham = "SP003", Mau_Sac = "Xanh Đậm",  Kich_Thuoc = "31",  So_Luong = 50,  Gia_BienThe = 610000 },

                // SP004 - Áo len croptop nữ
                new() { Id = 10, Ma_SanPham = "SP004", Mau_Sac = "Be",        Kich_Thuoc = "Freesize", So_Luong = 120, Gia_BienThe = 350000, Gia_Giam = 299000, PhanTramGiam = 15 },
                new() { Id = 11, Ma_SanPham = "SP004", Mau_Sac = "Đỏ Rượu",   Kich_Thuoc = "Freesize", So_Luong = 80,  Gia_BienThe = 350000 },

                // SP005 - Sơ mi kẻ nữ
                new() { Id = 12, Ma_SanPham = "SP005", Mau_Sac = "Trắng Kẻ Xanh", Kich_Thuoc = "Freesize", So_Luong = 100, Gia_BienThe = 380000, Gia_Giam = 329000, PhanTramGiam = 13 },

                // SP006 - Áo khoác kaki nam
                new() { Id = 13, Ma_SanPham = "SP006", Mau_Sac = "Xanh Rêu",  Kich_Thuoc = "M",   So_Luong = 60,  Gia_BienThe = 690000, Gia_Giam = 590000, PhanTramGiam = 14 },
                new() { Id = 14, Ma_SanPham = "SP006", Mau_Sac = "Xanh Rêu",  Kich_Thuoc = "L",   So_Luong = 80,  Gia_BienThe = 690000, Gia_Giam = 590000 },
                new() { Id = 15, Ma_SanPham = "SP006", Mau_Sac = "Đen",       Kich_Thuoc = "XL",  So_Luong = 45,  Gia_BienThe = 710000 },
                new() { Id = 16, Ma_SanPham = "SP006", Mau_Sac = "Be",        Kich_Thuoc = "M",   So_Luong = 55,  Gia_BienThe = 690000 }
            };
        }
    }
}