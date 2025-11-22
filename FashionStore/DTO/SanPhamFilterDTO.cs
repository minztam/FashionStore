namespace FashionStore.DTO
{
    public class SanPhamFilterDTO
    {
        public string? Ten { get; set; }
        public int? MaLoai { get; set; }
        public decimal? GiaTu { get; set; }
        public decimal? GiaDen { get; set; }
        public string? MauSac { get; set; }
        public string? KichThuoc { get; set; }
        public bool? ConHang { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // "GiaTang", "GiaGiam", "MoiNhat"
    }
}
