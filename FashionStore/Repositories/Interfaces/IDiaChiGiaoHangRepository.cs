using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDiaChiGiaoHangRepository
    {
        Task<ResponseMessageResult> GetAddress();
    }
}
