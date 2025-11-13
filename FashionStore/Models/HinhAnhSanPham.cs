using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    [Table("HinhAnhSanPham")]
    public class HinhAnhSanPham
    {
        [Key] // <-- khóa chính
        public int Ma_HinhAnh { get; set; }

        public string? Ma_SanPham { get; set; }
        public string? DuongDan { get; set; }

        // Navigation property
        [JsonIgnore]
        public SanPham? SanPham { get; set; }
    }
}