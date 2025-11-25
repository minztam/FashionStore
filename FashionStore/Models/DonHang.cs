using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("DonHang")]
    public class DonHang
    {
        [Key]
        public required string Ma_DonHang { get; set; }
        public int Ma_KhachHang { get; set; }
        public DateTime Ngay_Dat { get; set; } = DateTime.Now;
        public decimal Tong_Tien { get; set; }
        public string Trang_Thai { get; set; } = "Chờ xác nhận";
        public int Ma_PhuongThuc { get; set; }
        public string? Ma_Voucher { get; set; }

        // Navigation
        [ForeignKey("Ma_KhachHang")]
        public KhachHang? KhachHang { get; set; }
        [ForeignKey("Ma_PhuongThuc")]
        public PhuongThucThanhToan? PhuongThucThanhToan { get; set; }
        [ForeignKey("Ma_Voucher")]
        public Voucher? Voucher { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public int? Ma_DiaChi { get; set; }
        public DiaChiGiaoHang? DiaChiGiaoHang { get; set; }
    }
}
