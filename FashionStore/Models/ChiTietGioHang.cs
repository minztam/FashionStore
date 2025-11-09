using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class ChiTietGioHang
    {
        [Key]
        public int Ma_GioHang { get; set; }
        public required string Ma_SanPham { get; set; }
        public int So_Luong { get; set; }

        // Navigation property
        public GioHang? GioHang { get; set; }
        public SanPham? SanPham { get; set; }
    }
}