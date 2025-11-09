using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
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
        public KhachHang? KhachHang { get; set; }
        public PhuongThucThanhToan? PhuongThucThanhToan { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public Voucher? Voucher { get; set; }
    }
}
