using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class DiaChiGiaoHang
    {
        [Key]
        public int Ma_DiaChi { get; set; }

        public required string HoTen_NguoiNhan { get; set; }
        public required string SoDienThoai { get; set; }
        public required string DiaChi_ChiTiet { get; set; }
        public string? GhiChu { get; set; }

        public bool IsActive { get; set; }
        public  int Ma_KhachHang { get; set; }

        public KhachHang KhachHang { get; set; }
    }
}
