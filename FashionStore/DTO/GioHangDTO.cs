using FashionStore.Models;

namespace FashionStore.DTO
{
    public class GioHangDTO
    {
        public int Ma_GioHang { get; set; }
        public int Ma_KhachHang { get; set; }
        public List<ChiTietGioHangDTO> SanPhams { get; set; } = new();
        public int Tong_So_Luong => SanPhams.Sum(x => x.So_Luong);
        public decimal Tong_Tien => SanPhams.Sum(x => x.Thanh_Tien);
    }
}
