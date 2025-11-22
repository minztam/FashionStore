using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models
{
    public class PhuongThucThanhToan
    {
        [Key]
        public int Ma_PhuongThuc { get; set; }
        public required string Ten_PhuongThuc { get; set; }

        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}