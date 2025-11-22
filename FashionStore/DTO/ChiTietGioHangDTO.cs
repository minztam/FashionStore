using FashionStore.Models;

namespace FashionStore.DTO
{
    public class ChiTietGioHangDTO
    {
        public string Ma_SanPham { get; set; } = null!;
        public int Ma_BienThe {  get; set; }
        public string Ten_SanPham { get; set; } = null!;
        public string? Hinh_Anh { get; set; }
        public string Mau_Sac { get; set; } = null!;
        public string Kich_Thuoc { get; set; } = null!;
        public decimal Gia_Goc { get; set; }
        public decimal? Gia_Giam { get; set; }
        public int So_Luong { get; set; }
        public decimal Thanh_Tien => So_Luong * (Gia_Giam ?? Gia_Goc); // Tiền của sản phẩm này

    }

}