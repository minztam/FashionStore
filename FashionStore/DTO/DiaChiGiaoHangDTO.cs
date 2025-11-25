using Microsoft.AspNetCore.Mvc;

namespace FashionStore.DTO
{
    public class DiaChiGiaoHangDTO
    {
        public string Dia_Chi { get; set; } = string.Empty;
        public string? Ghi_Chu { get; set; }
        public string SDT { get; set; } = string.Empty;
        public string TenNguoiNhan { get; set; } = string.Empty; // Tên người nhận
        public int Ma_KhachHang { get; set; }
    }

}
