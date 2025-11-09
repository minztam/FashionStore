using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class DanhMuc
    {
        [Key]
        public required string Ma_DanhMuc { get; set; } = string.Empty;
        public required string Ten_DanhMuc { get; set; } = string.Empty;
        public string? Ma_DanhMucCha;
        public bool Trang_Thai { get; set; } = true;

        // Navigation property
        public DanhMuc? DanhMucCha { get; set; }
        public ICollection<DanhMuc>? DanhMucCon { get; set; }
        public ICollection<SanPham>? SanPhams { get; set; }

    }
}
