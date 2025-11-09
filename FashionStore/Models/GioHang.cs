using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class GioHang
    {
        [Key]
        public int Ma_GioHang { get; set; }
        public int Ma_KhachHang { get; set; }

        // Navigation property
        public KhachHang? KhachHang { get; set; }

        // Luôn khởi tạo danh sách để tránh null khi Add()
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
