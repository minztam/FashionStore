namespace FashionStore.Models.VNPay
{
    public class PaymentResult
    {
       
            public string? OrderId { get; set; }
            public string? TransactionNo { get; set; }
            public string? Amount { get; set; }
            public string? ResponseCode { get; set; }
            public string? Message { get; set; }
            public string? Voucher { get; set; }
            public int? Ma_KhachHang { get; set; } // nếu gửi lên
    }

}
