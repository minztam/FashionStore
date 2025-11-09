namespace FashionStore.DTO
{
    public class KhachHangDTO
    {
        // Id khách hàng, chỉ cần khi update
        public int Ma_TaiKhoan { get; set; }

        public string? Ho_Ten { get; set; }
        public string? So_Dien_Thoai { get; set; }
        public string? Dia_Chi { get; set; }
        public string? Hinh_Anh { get; set; }
    }
}
