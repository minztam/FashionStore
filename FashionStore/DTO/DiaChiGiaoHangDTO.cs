using Microsoft.AspNetCore.Mvc;

namespace FashionStore.DTO
{
    public class DiaChiGiaoHangDTO
    {
        public int Ma_DiaChi { get; set; }
        public string Dia_Chi { get; set; } = string.Empty;
        public string? Ghi_Chu { get; set; }
        public string SDT { get; set; } = string.Empty;
        public string TenantProfile { get; set; } = string.Empty; // Tên người nhận
    }

}
