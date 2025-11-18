using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public required string Ma_SanPham { get; set; }
        public required string Ten_SanPham { get; set; }
        public required string Ma_DanhMuc { get; set; }
        public string? Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; } = true;

        // Navigation property
        [ForeignKey("Ma_DanhMuc")]
        public DanhMuc? DanhMuc { get; set; }
        public ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();
        public ICollection<SanPhamBienThe> BienThes { get; set; } = new List<SanPhamBienThe>();
    }
}