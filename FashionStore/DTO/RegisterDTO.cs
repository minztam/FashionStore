namespace FashionStore.DTO
{
    public class RegisterDTO
    {
        public string Ten_DangNhap { get; set; } = string.Empty;
        public string Mat_Khau { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
    }
}
