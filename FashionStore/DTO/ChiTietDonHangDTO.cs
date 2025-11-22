namespace FashionStore.DTO
{
    public class ChiTietDonHangDTO
    {
        public string Ma_SanPham { get; set; } = null!;
        public string Ten_SanPham { get; set; } = null!;
        public string? Hinh_Anh { get; set; }           // THÊM HÌNH ẢNH
        public string? Mau_Sac { get; set; }            // THÊM MÀU
        public string? Kich_Thuoc { get; set; }         // THÊM SIZE
        public int So_Luong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
