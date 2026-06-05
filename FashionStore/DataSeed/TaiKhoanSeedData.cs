using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class TaiKhoanSeedData
    {
        public static IEnumerable<TaiKhoan> GetSeedData()
        {
            return new List<TaiKhoan>
            {
                new TaiKhoan
                {
                    Ma_TaiKhoan = 2,
                    Ten_DangNhap = "khachhang",               
                    Email = "khachhang@gmail.com",
                    Mat_Khau = "string",
                    Ma_VaiTro = "2222-2222-2222-2222",
                    Trang_Thai = true,
                    Ngay_Tao = DateTime.Parse("2025-12-02 17:30:42.2613937"),
                    Da_XacThuc = true,
                    Ma_XacThuc = "9pug2MZlj0m6HZPJ6Njolg==",
                    Han_XacThuc = DateTime.Parse("2025-12-03 10:30:42.2613963")
                },
                 new TaiKhoan
                {
                    Ma_TaiKhoan = 1,
                    Ten_DangNhap = "admin",
                    Email = "admin@gmail.com",
                    Mat_Khau = "string",
                    Ma_VaiTro = "1111-1111-1111-1111",
                    Trang_Thai = true,
                    Ngay_Tao = DateTime.Parse("2025-12-02 17:30:42.2613937"),
                    Da_XacThuc = true,
                    Ma_XacThuc = "9pug2MZlj0m6HZPJ6Njolg==",
                    Han_XacThuc = DateTime.Parse("2025-12-03 10:30:42.2613963")
                }, new TaiKhoan
                {
                    Ma_TaiKhoan = 3,
                    Ten_DangNhap = "staff",
                    Email = "staff@gmail.com",
                    Mat_Khau = "string",
                    Ma_VaiTro = "1111-2222-1111-2222",
                    Trang_Thai = true,
                    Ngay_Tao = DateTime.Parse("2025-12-02 17:30:42.2613937"),
                    Da_XacThuc = true,
                    Ma_XacThuc = "9pug2MZlj0m6HZPJ6Njolg==",
                    Han_XacThuc = DateTime.Parse("2025-12-03 10:30:42.2613963")
                }, new TaiKhoan
                {
                    Ma_TaiKhoan = 4,
                    Ten_DangNhap = "shipper",
                    Email = "shipper@gmail.com",
                    Mat_Khau = "string",
                    Ma_VaiTro = "3333-3333-3333-3333",
                    Trang_Thai = true,
                    Ngay_Tao = DateTime.Parse("2025-12-02 17:30:42.2613937"),
                    Da_XacThuc = true,
                    Ma_XacThuc = "9pug2MZlj0m6HZPJ6Njolg==",
                    Han_XacThuc = DateTime.Parse("2025-12-03 10:30:42.2613963")
                }

            };
} } }
