using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class HinhAnhSanPhamSeedData
    {
        public static IEnumerable<HinhAnhSanPham> GetSeedData()
        {
            return new List<HinhAnhSanPham>
            {
                // ==================== SP001 - Áo Sơ Mi Oxford Nam ====================
                new() { Ma_HinhAnh = 1,  Ma_SanPham = "SP001", DuongDan = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800" },
                new() { Ma_HinhAnh = 2,  Ma_SanPham = "SP001", DuongDan = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=800" },
                new() { Ma_HinhAnh = 3,  Ma_SanPham = "SP001", DuongDan = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800" },
                new() { Ma_HinhAnh = 4,  Ma_SanPham = "SP001", DuongDan = "https://images.unsplash.com/photo-1617114919317-8f6dce8e9df5?w=800" },

                // ==================== SP002 - Váy Maxi Lụa Hoa Nhí ====================
                new() { Ma_HinhAnh = 5,  Ma_SanPham = "SP002", DuongDan = "https://images.unsplash.com/photo-1594736797933-d0501ba2fe65?w=800" },
                new() { Ma_HinhAnh = 6,  Ma_SanPham = "SP002", DuongDan = "https://images.unsplash.com/photo-1563170351-be82bc888aa4?w=800" },
                new() { Ma_HinhAnh = 7,  Ma_SanPham = "SP002", DuongDan = "https://images.unsplash.com/photo-1550616541-96cf6efa3c5e?w=800" },
                new() { Ma_HinhAnh = 8,  Ma_SanPham = "SP002", DuongDan = "https://images.unsplash.com/photo-1585487000160-6ebcfceb0d03?w=800" },

                // ==================== SP003 - Quần Jeans Slimfit Nam ====================
                new() { Ma_HinhAnh = 9,  Ma_SanPham = "SP003", DuongDan = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=800" },
                new() { Ma_HinhAnh = 10, Ma_SanPham = "SP003", DuongDan = "https://images.unsplash.com/photo-1602293589930-45aad59ba3e4?w=800" },
                new() { Ma_HinhAnh = 11, Ma_SanPham = "SP003", DuongDan = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=800" },

                // ==================== SP004 - Áo Len Croptop Nữ ====================
                new() { Ma_HinhAnh = 12, Ma_SanPham = "SP004", DuongDan = "https://images.unsplash.com/photo-1611318440154-8f60a0b9f512?w=800" },
                new() { Ma_HinhAnh = 13, Ma_SanPham = "SP004", DuongDan = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800" },
                new() { Ma_HinhAnh = 14, Ma_SanPham = "SP004", DuongDan = "https://images.unsplash.com/photo-1617019114583-affb34d1b3cd?w=800" },

                // ==================== SP005 - Sơ Mi Kẻ Sọc Nữ ====================
                new() { Ma_HinhAnh = 15, Ma_SanPham = "SP005", DuongDan = "https://images.unsplash.com/photo-1598550874175-4d0ef436c909?w=800" },
                new() { Ma_HinhAnh = 16, Ma_SanPham = "SP005", DuongDan = "https://images.unsplash.com/photo-1612902376491-7a8a9b42425f?w=800" },
                new() { Ma_HinhAnh = 17, Ma_SanPham = "SP005", DuongDan = "https://images.unsplash.com/photo-1622473596033-9d8d1e7e6a5a?w=800" },

                // ==================== SP006 - Áo Khoác Kaki Nam Có Mũ ====================
                new() { Ma_HinhAnh = 18, Ma_SanPham = "SP006", DuongDan = "https://images.unsplash.com/photo-1551029506-0807df4e2031?w=800" },
                new() { Ma_HinhAnh = 19, Ma_SanPham = "SP006", DuongDan = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=800" },
                new() { Ma_HinhAnh = 20, Ma_SanPham = "SP006", DuongDan = "https://images.unsplash.com/photo-1552374196-c4e7ffc6e126?w=800" },
                new() { Ma_HinhAnh = 21, Ma_SanPham = "SP006", DuongDan = "https://images.unsplash.com/photo-1604176354204-9268737828e4?w=800" },

                // ==================== Thêm vài sản phẩm bonus mở rộng sau này ====================
                // SP007 - Giày Sneaker Trắng
                // new() { Ma_HinhAnh = 22, Ma_SanPham = "SP007", DuongDan = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=800" },
                // SP008 - Túi Xách Nữ
                // new() { Ma_HinhAnh = 23, Ma_SanPham = "SP008", DuongDan = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=800" },
            };
        }
    }
}