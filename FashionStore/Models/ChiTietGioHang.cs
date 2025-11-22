using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("ChiTietGioHang")]
    public class ChiTietGioHang
    {
        [Key]
        public int Ma_GioHang { get; set; }
        public required string Ma_SanPham { get; set; }
        public int Ma_BienThe { get; set; }
        public int So_Luong { get; set; }

        // Navigation property
        [ForeignKey("Ma_GioHang")]
        public GioHang? GioHang { get; set; }

        [ForeignKey("Ma_SanPham")]
        public SanPham? SanPham { get; set; }

        [ForeignKey("Ma_BienThe")]
        public SanPhamBienThe? BienThe { get; set; }
    }
}