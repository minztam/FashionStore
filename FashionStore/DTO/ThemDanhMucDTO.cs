using FashionStore.Models;

namespace FashionStore.DTO
{
    public class ThemDanhMucDTO
    {
        public string Ten_DanhMuc { get; set; } = null!;
        public string? Ma_DanhMucCha { get; set; }
        public bool Trang_Thai { get; set; }
    }
}
