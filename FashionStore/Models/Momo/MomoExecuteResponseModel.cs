namespace FashionStore.Models.Momo
{
    public class MomoExecuteResponseModel
    {
        public string OrderId { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string OrderInfo { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
