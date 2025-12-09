using FashionStore.Models;

namespace FashionStore.DTO
{
    public class DonHangDTO
    {
        public string? Ma_DonHang { get; set; }
        public int Ma_KhachHang { get; set; }
        public int Ma_DiaChi { get; set; }
        public int? Ma_Shipper { get; set; }
        public int? Ma_NhanVien { get; set; }
        public DateTime Ngay_Dat { get; set; }
        public decimal Tong_Tien { get; set; }
        public string Trang_Thai { get; set; } = string.Empty;
        public int Ma_PhuongThuc { get; set; }
        public string? Ten_PhuongThuc { get; set; }
        public string? Ma_Voucher { get; set; }
        public decimal GiamGia { get; set; }
        public DiaChiGiaoHang? DiaChi { get; set; }
        public NhanVien? NhanVien { get; set; }
        public Shipper? Shipper { get; set; }

        public List<ChiTietDonHangDTO> ChiTiet { get; set; } = new();
    }
}
