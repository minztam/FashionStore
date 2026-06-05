using static FashionStore.Controllers.ThanhToanController;

namespace FashionStore.DTO
{
    public class ThanhToanCODRequest
    {
        public int Ma_KhachHang { get; set; }
        public int Ma_DiaChi { get; set; }
        public string? Ma_Voucher { get; set; }

        // Danh sách biến thể muốn thanh toán và số lượng
        public List<CheckoutItem> ChiTiet { get; set; } = new();
    }
}
