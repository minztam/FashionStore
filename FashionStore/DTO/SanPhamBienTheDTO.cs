namespace FashionStore.DTO
{
    public class SanPhamBienTheDTO
    {
        public int Id { get; set; }
        public string Mau_Sac { get; set; } = string.Empty;
        public string Kich_Thuoc { get; set; } = string.Empty;
        public string? HinhAnh { get; set; } 
        public int So_Luong { get; set; }
        public decimal Gia_BienThe { get; set; }
        public decimal? Gia_Giam { get; set; } // Giá sau khi giảm (optional)
        public int? PhanTramGiam { get; set; } // Phần trăm giảm (optional)
        public bool? Trang_Thai { get; set; }
    }
}
