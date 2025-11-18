using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("GioHang")]
    public class GioHang
    {
        [Key]
        public int Ma_GioHang { get; set; }
        public int Ma_KhachHang { get; set; }

        // Navigation property
        [ForeignKey("Ma_KhachHang")]
        public KhachHang? KhachHang { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
