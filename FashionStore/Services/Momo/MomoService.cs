using FashionStore.Models;
using FashionStore.Models.Momo;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace FashionStore.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly MomoOptionModel _opt;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _opt = options.Value;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            string requestId = Guid.NewGuid().ToString();   // KHÔNG được dùng orderId
            long amount = (long)model.Amount;

            // ======== RAW SIGNATURE CHUẨN MOMO V2 ========
            string rawSignature =
                $"accessKey={_opt.AccessKey}" +
                $"&amount={amount}" +
                $"&extraData=" +
                $"&ipnUrl={_opt.NotifyUrl}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={model.OrderInfo}" +
                $"&partnerCode={_opt.PartnerCode}" +
                $"&redirectUrl={_opt.ReturnUrl}" +
                $"&requestId={requestId}" +
                $"&requestType=captureWallet";

            string signature = CreateSHA256Signature(rawSignature, _opt.SecretKey);

            // ======== REQUEST BODY CHUẨN MOMO ====
            var requestBody = new
            {
                partnerCode = _opt.PartnerCode,
                partnerName = "FashionStore",
                storeId = "FashionStore",
                requestId = requestId,
                amount = amount,
                orderId = model.OrderId,
                orderInfo = model.OrderInfo,
                redirectUrl = _opt.ReturnUrl,
                ipnUrl = _opt.NotifyUrl,
                lang = "vi",
                extraData = "",
                requestType = "captureWallet",
                signature = signature
            };

            string json = JsonConvert.SerializeObject(requestBody);

            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_opt.ApiEndpoint, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"MoMo API Error: {responseText}");
            }

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(responseText)
                ?? new MomoCreatePaymentResponseModel { Message = "Empty response" };
        }

        private string CreateSHA256Signature(string rawData, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(rawData);

            using var hmac = new HMACSHA256(keyBytes);
            byte[] hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public MomoExecuteResponseModel PaymentExecute(IQueryCollection query)
        {
            return new MomoExecuteResponseModel
            {
                OrderId = query["orderId"],
                Amount = query["amount"],
                OrderInfo = query["orderInfo"],
                ErrorCode = query["errorCode"],
                Message = query["message"],
                Signature = query["signature"]
            };
        }
    }
}
