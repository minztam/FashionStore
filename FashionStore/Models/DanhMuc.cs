using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        [Key]
        public required string Ma_DanhMuc { get; set; } = string.Empty;
        public required string Ten_DanhMuc { get; set; } = string.Empty;  
        public string? Ma_DanhMucCha { get; set; }
        public bool Trang_Thai { get; set; } = true;

        // Navigation property
        [JsonIgnore]
        public DanhMuc? DanhMucCha { get; set; }
        public ICollection<DanhMuc>? DanhMucCon { get; set; }
        public ICollection<SanPham>? SanPhams { get; set; }

    }
}
