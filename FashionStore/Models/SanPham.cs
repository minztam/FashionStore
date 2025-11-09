using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class SanPham
    {
        [Key]
        public required string Ma_SanPham { get; set; }
        public required string Ten_SanPham { get; set; }
        public required string Ma_DanhMuc { get; set; }
        public string? Mo_Ta { get; set; }
        public decimal Gia { get; set; }
        public decimal? Gia_Gia { get; set; }
        public required int So_Luong { get; set; }
        public string? Mau_Sac { get; set; }
        public string? Kich_Thuoc { get; set; }
        public bool Trang_Thai { get; set; } = true;

        // Navigation property
        public DanhMuc? DanhMuc { get; set; }
        public ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}