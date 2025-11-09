using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class Voucher
    {
        [Key]
        public required string Ma_Voucher { get; set; }
        public int? Giam_PhanTram { get; set; }
        public decimal? Giam_Tien { get; set; }
        public decimal? GiaTri_ToiThieu { get; set; }
        public int? So_LanDung { get; set; }
        public DateTime? Ngay_BatDau { get; set; }
        public DateTime? Ngay_KetThuc { get; set; }
        public bool Trang_Thai { get; set; } = true;

        // Navigation
        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
