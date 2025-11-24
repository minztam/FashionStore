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
                    Ten_SanPham = "Áo Sơ Mi Oxford Nam Cao Cấp",
                    Ma_DanhMuc = "DM001A", // Áo nam
                    Mo_Ta = "Chất vải Oxford nhập khẩu, thoáng mát, không nhăn, form dáng chuẩn công sở và dự tiệc.",
                    Trang_Thai = true
                },
                new SanPham
                {
                    Ma_SanPham = "SP002",
                    Ten_SanPham = "Váy Maxi Lụa Hoa Nhí Thanh Lịch",
                    Ma_DanhMuc = "DM002B", // Váy nữ
                    Mo_Ta = "Lụa cao cấp mát mẻ, họa tiết hoa nhí vintage, dáng dài thướt tha, đi biển hay dự tiệc đều đẹp.",
                    Trang_Thai = true
                },
                new SanPham
                {
                    Ma_SanPham = "SP003",
                    Ten_SanPham = "Quần Jeans Nam Slimfit Rách Gối",
                    Ma_DanhMuc = "DM001B", // Quần nam
                    Mo_Ta = "Jeans co giãn 4 chiều, form slimfit tôn dáng, rách gối phong cách streetwear trẻ trung.",
                    Trang_Thai = true
                },
                new SanPham
                {
                    Ma_SanPham = "SP004",
                    Ten_SanPham = "Áo Len Croptop Nữ Dáng Rộng",
                    Ma_DanhMuc = "DM002A", // Áo nữ
                    Mo_Ta = "Len mỏng nhẹ, dáng croptop tay dài, phối đồ mùa thu đông cực xinh, freesize từ 45-60kg.",
                    Trang_Thai = true
                },
                // sản phẩm 
                new SanPham
                {
                    Ma_SanPham = "SP005",
                    Ten_SanPham = "Sơ Mi Kẻ Sọc Nữ Form Rộng",
                    Ma_DanhMuc = "DM002A",
                    Mo_Ta = "Sơ mi kẻ caro hàn quốc, form rộng oversize, mặc mát cả mùa hè.",
                    Trang_Thai = true
                },
                new SanPham
                {
                    Ma_SanPham = "SP006",
                    Ten_SanPham = "Áo Khoác Kaki Nam Có Mũ",
                    Ma_DanhMuc = "DM001A", // Áo khoác nam
                    Mo_Ta = "Kaki dày dặn, có mũ tháo rời, chống nắng chống mưa nhẹ, form dáng trẻ trung.",
                    Trang_Thai = true
                }
            };
        }
    }
}