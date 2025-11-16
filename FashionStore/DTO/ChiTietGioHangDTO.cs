using FashionStore.Models;

namespace FashionStore.DTO
{
    public class ChiTietGioHangDTO
    {
        public string Ma_SanPham { get; set; } = string.Empty;
        public string Ten_SanPham { get; set; } = string.Empty;
        public string Hinh_Anh { get; set; } = string.Empty;

        public decimal Gia { get; set; }        // giá gốc
        public decimal Gia_Giam { get; set; }   // số tiền giảm

        public int So_Luong { get; set; }

        // Giá sau giảm
        public decimal GiaSauGiam => Math.Max(0, Gia - Gia_Giam);

        // Thành tiền theo số lượng
        public decimal ThanhTien => (Gia - Gia_Giam) * So_Luong;

    }

}