using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class SanPhamSeedData
    {
        public static IEnumerable<SanPham> GetSeedData()
        {
            return new List<SanPham>
            {
              new SanPham
            {
        Ma_SanPham = "SP001",
        Ten_SanPham = "Áo Sơ Mi Oxford Nam",
        Ma_DanhMuc = "DM001A", // Áo nam
        Mo_Ta = "Áo sơ mi vải Oxford cao cấp, thoáng mát, phong cách công sở trẻ trung.",
        Gia = 450000,
        Gia_Giam = 399000, // Có giảm giá
        So_Luong = 150,
        Mau_Sac = "Trắng",
        Kich_Thuoc = "M, L, XL",
        Trang_Thai = true
    },
    
    // 2. Sản phẩm Nữ - Váy Maxi (DM002B)
    new SanPham
    {
        Ma_SanPham = "SP002",
        Ten_SanPham = "Váy Maxi Lụa Hoa Nhí",
        Ma_DanhMuc = "DM002B", // Váy nữ
        Mo_Ta = "Váy maxi chất liệu lụa mềm mại, họa tiết hoa nhí, thích hợp đi biển hoặc dạo phố.",
        Gia = 780000,
        Gia_Giam = null, // Không giảm giá
        So_Luong = 80,
        Mau_Sac = "Vàng, Xanh",
        Kich_Thuoc = "S, M, L",
        Trang_Thai = true
    },
    
    // 3. Sản phẩm Nam - Quần Jeans (DM001B)
    new SanPham
    {
        Ma_SanPham = "SP003",
        Ten_SanPham = "Quần Jeans Slimfit Đen",
        Ma_DanhMuc = "DM001B", // Quần nam
        Mo_Ta = "Quần jeans co giãn nhẹ, form slimfit hiện đại, dễ phối đồ.",
        Gia = 550000,
        Gia_Giam = 450000,
        So_Luong = 120,
        Mau_Sac = "Đen",
        Kich_Thuoc = "28, 29, 30, 31, 32",
        Trang_Thai = true
    },
    
    // 4. Sản phẩm Nữ - Áo Len Dáng Rộng (DM002A)
    new SanPham
    {
        Ma_SanPham = "SP004",
        Ten_SanPham = "Áo Len Croptop Tay Dài",
        Ma_DanhMuc = "DM002A", // Áo nữ
        Mo_Ta = "Áo len mỏng, kiểu dáng croptop, phong cách Hàn Quốc.",
        Gia = 320000,
        Gia_Giam = null,
        So_Luong = 95,
        Mau_Sac = "Be, Đỏ",
        Kich_Thuoc = "Freesize",
        Trang_Thai = true
    }
            };
        }
    }
}
