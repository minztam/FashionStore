namespace FashionStore.DTO
{
    public class DanhMucDTO
    {
        public string Ma_DanhMuc { get; set; } = null!;     
        public string Ten_DanhMuc { get; set; } = null!;
        public string? Ma_DanhMucCha { get; set; }
        public string? Ten_DanhMucCha { get; set; }
        public bool Trang_Thai { get; set; }
    }
}
