using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public int Ma_TaiKhoan { get; set; }
        public required string Ten_DangNhap { get; set; }
        public string? Email { get; set; }
        public required string Mat_Khau { get; set; }
        public required string Ma_VaiTro { get; set; }
        public bool Trang_Thai { get; set; } =  true;
        public DateTime? Ngay_Tao { get; set; } = DateTime.Now;
        public bool Da_XacThuc { get; set; } = true;
        public string? Ma_XacThuc { get; set; } = string.Empty;
        public DateTime? Han_XacThuc { get; set; }

        // Navigation property

        [ForeignKey("Ma_VaiTro")]
        public VaiTro? VaiTro { get; set; }
        [JsonIgnore]
        public NhanVien? NhanVien { get; set; }
        [JsonIgnore]
        public KhachHang? KhachHang { get; set; }
    }
}