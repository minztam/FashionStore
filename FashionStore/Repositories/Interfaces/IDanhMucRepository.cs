using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDanhMucRepository
    {
        Task<ResponseMessageResult> GetAllAsync();
        Task<DanhMucDTO?> GetByIdAsync(string maDanhMuc);
        Task<IEnumerable<DanhMucDTO>> SearchAsync(string? keyword);
        Task<bool> AddAsync(DanhMuc danhMuc);
        Task<bool> UpdateAsync(DanhMuc danhMuc);
        Task<bool> DeleteAsync(string id);
        Task<ResponseMessageResult> GetTreeAsync();
        Task<bool> ToggleStatusAsync(string id);
    }
}
