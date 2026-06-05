using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDiaChiGiaoHangRepository
    {
        Task<ResponseMessageResult> GetAddress(int maKhachHang);
        Task<ResponseMessageResult> AddAddress(DiaChiGiaoHangDTO model);
        Task<ResponseMessageResult> UpdateAddress(int id, DiaChiGiaoHangDTO model);
        Task<ResponseMessageResult> DeleteAddress(int id);
    }
}
