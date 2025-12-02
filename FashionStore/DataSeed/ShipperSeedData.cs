using FashionStore.Migrations;
using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public class ShipperSeedData
    {
        public static IEnumerable<Shipper> GetSeedData()
        {
            return new List<Shipper>
            {
                new Shipper
                {
                    Ma_Shipper = 1,
                    HinhAnh = "https://cdnphoto.dantri.com.vn/PZJhMVnXWQcyRwL7GOzw539WApw=/thumb_w/1020/2024/02/01/ghtk-crop-edited-1706744665204.jpeg",
                    Ma_TaiKhoan = 4,
                    Ten_DayDu = "Trương Minh Tâm",
                    SoDienThoai = "0982343222",
                    BienSoXe = "69N4-9999",
                    TrangThai = "online"
                }
            };
        }
    }
}
