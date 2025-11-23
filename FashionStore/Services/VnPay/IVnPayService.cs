using FashionStore.Models.VNPay;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Services.VnPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        ResponseMessageResult PaymentExecute(IQueryCollection collection);
    }
}
