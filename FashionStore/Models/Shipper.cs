using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class Shipper
    {
        [Key]
        public int Ma_Shipper { get; set; }
        public string? HinhAnh { get; set; }
        public int Ma_TaiKhoan { get; set; }

        public string? Ten_DayDu { get; set; }
        public string? SoDienThoai { get; set; }
        public string? BienSoXe { get; set; }

        public string? TrangThai { get; set; } 

        // Navigation
        public TaiKhoan TaiKhoan { get; set; } = null!;
    }

}
