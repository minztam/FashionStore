using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("SanPhamBienThe")]
    public class SanPhamBienThe
    {
        [Key]
        public int Id { get; set; }
        public string Ma_SanPham { get; set; } = null!;
        public string Mau_Sac { get; set; } = null!;
        public string Kich_Thuoc { get; set; } = null!;
        public int So_Luong { get; set; }
        public decimal Gia_BienThe { get; set; }
        public decimal? Gia_Giam { get; set; } // giá sau khi giảm (optional)
        public int? PhanTramGiam { get; set; } // giảm %

        // Navigation property
        [ForeignKey("Ma_SanPham")]
        public SanPham? SanPham { get; set; }
    }
}
