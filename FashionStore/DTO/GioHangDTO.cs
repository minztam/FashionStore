using FashionStore.Models;

namespace FashionStore.DTO
{
    public class GioHangDTO
    {
        public int Ma_GioHang { get; set; }
        public int Ma_KhachHang { get; set; }

        public List<ChiTietGioHangDTO> SanPhams { get; set; } = new();

        public decimal TongTien => SanPhams.Sum(x => x.ThanhTien);
    }
}
