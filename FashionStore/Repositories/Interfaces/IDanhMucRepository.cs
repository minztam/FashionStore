using FashionStore.DTO;
using FashionStore.Models;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDanhMucRepository
    {
        Task<IEnumerable<DanhMucDTO>> GetAllAsync();
        Task<DanhMucDTO?> GetByIdAsync(string maDanhMuc);
        Task<IEnumerable<DanhMucDTO>> SearchAsync(string? keyword);
        Task<bool> AddAsync(DanhMuc danhMuc);
        Task<bool> UpdateAsync(DanhMuc danhMuc);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<DanhMuc>> GetTreeAsync();
        Task<bool> ToggleStatusAsync(string id);
    }
}
