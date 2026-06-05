using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IShipperRepository
    {
        Task<ResponseMessageResult> GetAllAsync();
        Task<ResponseMessageResult> GetByIdAsync(int maShipper);
        Task<ResponseMessageResult> AddAsync(ShipperRequestDTO shipper);
        Task<ResponseMessageResult> UpdateAsync(int maShipper, ShipperRequestDTO shipper);
        Task<ResponseMessageResult> DeleteAsync(int maShipper);
        Task<ResponseMessageResult> ToggleStatusAsync(int maShipper);
    }
}
