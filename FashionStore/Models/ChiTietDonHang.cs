using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key]
        public required string Ma_DonHang { get; set; }
        public required string Ma_SanPham { get; set; }
        public int So_Luong { get; set; }
        public decimal DonGia { get; set; }

        // Navigation
        [ForeignKey("Ma_DonHang")]
        public DonHang? DonHang { get; set; }
        [ForeignKey("Ma_SanPham")]
        public SanPham? SanPham { get; set; }
    }
}