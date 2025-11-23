namespace FashionStore.Models.VNPay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; } = "FashionStore"; // 190001 = thời trang, mặc định
        public decimal Amount { get; set; } // Dùng decimal thay double → chính xác tiền tệ
        public string OrderDescription { get; set; } = "Thanh toan don hang tai Fashion Store";
        public string Name { get; set; } = "Khach hang"; // Tên khách hoặc mã đơn
        public string OrderId { get; set; } = DateTime.Now.Ticks.ToString(); // Bắt buộc có để lưu DB
    }
}
