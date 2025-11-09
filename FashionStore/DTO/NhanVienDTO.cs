namespace FashionStore.DTO
{
    public class NhanVienDTO
    {
        // Dùng cho create/update: bắt nhập Ma_TaiKhoan để liên kết với TaiKhoan đã có
        public int Ma_TaiKhoan { get; set; }

        // Có thể null khi update (nếu không muốn thay đổi)
        public string? Ho_Ten { get; set; }
        public string? So_Dien_Thoai { get; set; }
        public string? Dia_Chi { get; set; }
        public string? Hinh_Anh { get; set; }

    }
}
