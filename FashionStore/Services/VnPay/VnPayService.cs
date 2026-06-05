using FashionStore.DTO;
using FashionStore.Library;
using FashionStore.Models.VNPay;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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

            // Standardize orderInfo format: PAY|OrderId|Voucher|MaKhachHang
            string voucherPart = model.Voucher ?? "NONE";
            string khPart = model.Ma_KhachHang.HasValue ? model.Ma_KhachHang.Value.ToString() : "0";
            string orderInfo = $"PAY|{model.OrderId}|{voucherPart}|{khPart}";
            vnpay.AddRequestData("vnp_OrderInfo", orderInfo);

            vnpay.AddRequestData("vnp_OrderType", model.OrderType ?? "");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]!);
            vnpay.AddRequestData("vnp_TxnRef", model.OrderId);

            return vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:HashSecret"]!);
        }

        public ResponseMessageResult PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();

            // copy vnp_* params
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    vnpay.AddResponseData(key, value.ToString());
            }

            var vnp_SecureHash = collection["vnp_SecureHash"].ToString();
            bool isValidSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["Vnpay:HashSecret"]);

            if (!isValidSignature)
                return _response.SetFail("Chữ ký không hợp lệ!", 400);

            string responseCode = vnpay.GetResponseData("vnp_ResponseCode") ?? "";
            string orderId = vnpay.GetResponseData("vnp_TxnRef") ?? "";
            string transactionNo = vnpay.GetResponseData("vnp_TransactionNo") ?? "";
            string amountRaw = vnpay.GetResponseData("vnp_Amount") ?? "0";
            long amount = long.TryParse(amountRaw, out var tmp) ? tmp / 100 : 0;

            // Parse standardized vnp_OrderInfo => PAY|OrderId|Voucher|MaKhachHang
            string orderInfo = vnpay.GetResponseData("vnp_OrderInfo") ?? "";
            string? voucher = null;
            int? maKhachHang = null;

            if (!string.IsNullOrEmpty(orderInfo))
            {
                var parts = orderInfo.Split('|', StringSplitOptions.RemoveEmptyEntries);
                // parts[0] = PAY
                if (parts.Length >= 3)
                {
                    if (parts[2] != "NONE")
                        voucher = parts[2];
                }
                if (parts.Length >= 4 && int.TryParse(parts[3], out var kh))
                    maKhachHang = kh;
            }

            var data = new PaymentResult
            {
                OrderId = orderId,
                TransactionNo = transactionNo,
                Amount = amount.ToString("N0"),
                ResponseCode = responseCode,
                Message = responseCode == "00" ? "Thanh toán thành công!" : GetVnpayMessage(responseCode),
                Voucher = voucher,
                Ma_KhachHang = maKhachHang
            };

            if (responseCode == "00")
                return _response.SetSuccess("Thanh toán VNPAY thành công!", data);

            // preserve data on fail too (do not rely only on SetFail default)
            var fail = _response.SetFail("Thanh toán thất bại!", 400);
            fail.Data = data;
            return fail;
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
