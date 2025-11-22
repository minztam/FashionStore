namespace FashionStore.Models.Momo
{
    public class MomoCreatePaymentResponseModel
    {
        public string? PartnerCode { get; set; }
        public string? AccessKey { get; set; }
        public string? RequestId { get; set; }
        public string? Amount { get; set; }
        public string? OrderId { get; set; }
        public string? OrderInfo { get; set; }

        public string? RedirectUrl { get; set; }
        public string? PayUrl { get; set; }
        public string? QrCodeUrl { get; set; }    // thêm
        public string? Deeplink { get; set; }     // thêm

        public string? RequestType { get; set; }
        public int ErrorCode { get; set; }
        public string? Message { get; set; }
        public string? LocalMessage { get; set; }
        public string? Signature { get; set; }
    }
}
