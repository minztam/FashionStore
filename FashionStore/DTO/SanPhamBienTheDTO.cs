namespace FashionStore.DTO
{
    public class SanPhamBienTheDTO
    {
        public string Mau_Sac { get; set; } = string.Empty;
        public string Kich_Thuoc { get; set; } = string.Empty;
        public int So_Luong { get; set; }
        public decimal Gia_BienThe { get; set; }
        public decimal? Gia_Giam { get; set; } // Giá sau khi giảm (optional)
        public int? PhanTramGiam { get; set; } // Phần trăm giảm (optional)
    }
}
