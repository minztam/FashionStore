namespace FashionStore.Models.VNPay
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;     // vnp_TransactionNo
        public string OrderId { get; set; } = string.Empty;           // vnp_TxnRef (mã đơn hàng của mình)
        public string PaymentMethod { get; set; } = "VNPAY";
        public string PaymentId { get; set; } = string.Empty;         // vnp_TransactionNo
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty; // 00 = thành công
    }
}
