using FashionStore.Library;
using FashionStore.Models.VNPay;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Services.VnPay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly ResponseMessageResult _response;

        public VnPayService(IConfiguration configuration, ResponseMessageResult response)
        {
            _configuration = configuration;
            _response = response;
        }
        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var vnpay = new VnPayLibrary();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]!);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            vnpay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]!);
            vnpay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]!);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]!);
            vnpay.AddRequestData("vnp_Amount", ((long)(model.Amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]!);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]!);
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {model.OrderId} - {model.Name}");
            vnpay.AddRequestData("vnp_OrderType", model.OrderType);
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]!);
            vnpay.AddRequestData("vnp_TxnRef", model.OrderId);

            return vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:HashSecret"]!);
        }

        public ResponseMessageResult PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    vnpay.AddResponseData(key, value.ToString());
            }

            var vnp_SecureHash = collection.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var isValidSignature = vnpay.ValidateSignature(vnp_SecureHash!, _configuration["Vnpay:HashSecret"]!);

            if (!isValidSignature)
                return _response.SetFail("Chữ ký không hợp lệ!", 400);

            var responseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var orderId = vnpay.GetResponseData("vnp_TxnRef");
            var transactionNo = vnpay.GetResponseData("vnp_TransactionNo");
            var amount = vnpay.GetResponseData("vnp_Amount");

            var result = new
            {
                OrderId = orderId,
                TransactionNo = transactionNo,
                Amount = (long.Parse(amount) / 100).ToString("N0"),
                ResponseCode = responseCode,
                Message = responseCode == "00" ? "Thanh toán thành công!" : GetVnpayMessage(responseCode)
            };

            return responseCode == "00"
                ? _response.SetSuccess("Thanh toán VNPAY thành công!", result)
                : _response.SetFail($"Thanh toán thất bại: {GetVnpayMessage(responseCode)}", 400);
        }
        private string GetIpAddress(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            return string.IsNullOrEmpty(ip) || ip == "::1" ? "127.0.0.1" : ip;
        }

        private string GetVnpayMessage(string code) => code switch
        {
            "00" => "Giao dịch thành công",
            "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ",
            "09" => "Giao dịch không thành công do thẻ/tài khoản bị khóa",
            "10" => "Giao dịch không thành công do lỗi ngân hàng",
            "11" => "Giao dịch không thành công do hết hạn",
            "12" => "Giao dịch không thành công do thẻ bị khóa",
            "13" => "Giao dịch không thành công do sai mật khẩu",
            "24" => "Giao dịch bị hủy bởi người dùng",
            _ => "Lỗi không xác định từ VNPAY"
        };
    }
}
