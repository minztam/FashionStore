using FashionStore.Models;
using FashionStore.Models.Momo;

namespace FashionStore.Services.Momo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecute(IQueryCollection query);
    }
}
