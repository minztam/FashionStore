using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public int Ma_NhanVien { get; set; }
        public int Ma_TaiKhoan { get; set; }
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? Hinh_Anh { get; set; }

        public TaiKhoan? TaiKhoan { get; set; }
    }
}