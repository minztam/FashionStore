namespace FashionStore.DTO
{
    public class SanPhamDTO
    {
        public string Ten_SanPham { get; set; } = string.Empty;
        public string Ma_DanhMuc { get; set; } = string.Empty;
        public string? Mo_Ta { get; set; }
        public decimal Gia { get; set; }
        public decimal? Gia_Giam { get; set; }
        public int So_Luong { get; set; }
        public string? Mau_Sac { get; set; }
        public string? Kich_Thuoc { get; set; }
        public bool Trang_Thai { get; set; } = true;

        public List<HinhAnhDTO>? HinhAnhs { get; set; }
    }
}
