using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    [Table("VaiTro")]
    public class VaiTro
    {
        [Key]
        public required string Ma_VaiTro { get; set; }
        public required string Ten_VaiTro { get; set; }

        // Navigation property
        [JsonIgnore] // ← bỏ TaiKhoans khi serialize JSON
        public ICollection<TaiKhoan>? TaiKhoans { get; set; }
    }
}
