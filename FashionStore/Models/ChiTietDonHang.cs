using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public required string Ma_DonHang { get; set; }
        public required string Ma_SanPham { get; set; }
        public int So_Luong { get; set; }
        public decimal DonGia { get; set; }

        // Navigation
        public DonHang? DonHang { get; set; }
        public SanPham? SanPham { get; set; }
    }
}