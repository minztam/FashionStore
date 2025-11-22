namespace FashionStore.DTO
{
    public class DanhMucTreeDTO
    {
        public string Ma_DanhMuc { get; set; } = null!;
        public string Ten_DanhMuc { get; set; } = null!;
        public string? Ma_DanhMucCha { get; set; }
        public List<DanhMucTreeDTO> DanhMucCon { get; set; } = new();
    }
}
