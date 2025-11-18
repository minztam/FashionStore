namespace FashionStore.DTO
{
    public class ChiTietDonHangDTO
    {
        public string Ma_SanPham { get; set; } = string.Empty;
        public string? Ten_SanPham { get; set; }
        public int So_Luong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => So_Luong * DonGia;
    }
}
