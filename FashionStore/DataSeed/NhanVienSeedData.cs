using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public class NhanVienSeedData
    {
        public static IEnumerable<NhanVien> GetSeedData()
        {
            return new List<NhanVien>
            {new NhanVien
                        {
                            Ma_NhanVien = 2,
                            Ma_TaiKhoan = 3,
                            HoTen = "Nguyễn Đình Văn",
                            SoDienThoai = "0982343544",
                            DiaChi = "Sài Gòn",
                            Hinh_Anh = "https://cdn.24h.com.vn/upload/2-2021/images/2021-04-09/untitled-8-1617960812-512-width650height900.jpg"
                        },
            };
        }
    }
}
