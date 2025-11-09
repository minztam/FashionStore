using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class HinhAnhSanPham
    {
        [Key] // <-- khóa chính
        public int MaHinhAnh { get; set; }

        public string? MaSanPham { get; set; }
        public string? DuongDan { get; set; }

        // Navigation property
        public SanPham? SanPham { get; set; }
    }
}