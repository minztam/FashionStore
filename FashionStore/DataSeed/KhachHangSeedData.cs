using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public class KhachHangSeedData
    {
        public static IEnumerable<KhachHang> GetSeedData()
        {
            return new List<KhachHang>
            {
         new KhachHang{
                    Ma_KhachHang = 1,
                    Ma_TaiKhoan = 2,
                    HoTen = "Trịnh Anh Đức",
                    SoDienThoai = "0989105436",
                    DiaChi = "Thanh Hóa",
                    Hinh_Anh = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSk8nOS9W4aP2bq6MY4K-HlIusxbKz4Bw1PEw&s"
                },
                            };
            }
            }
}
