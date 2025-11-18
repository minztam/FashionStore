using FashionStore.DTO;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDonHangRepository
    {
        Task<ResponseMessageResult> TaoDonHangAsync(DonHangDTO dto);
        Task<ResponseMessageResult> GetDonHang(string maDH);
    }
}
