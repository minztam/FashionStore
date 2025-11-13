using FashionStore.DTO;
using FashionStore.Models;

namespace FashionStore.Repositories.Interfaces
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham?> GetByIdAsync(string id);
        Task<bool> CreateAsync(SanPham sp, List<HinhAnhDTO>? hinhAnhs);
        Task<bool> UpdateAsync(string maSanPham, SanPhamDTO dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ToggleStatusAsync(string id);

        // Cập nhật riêng từng phần (PATCH)
        Task<bool> PatchAsync(string id, SanPhamDTO dto);

        // Quản lý hình ảnh
        Task<bool> AddImageAsync(HinhAnhSanPham img);
        Task<bool> DeleteImageAsync(int id);
    }
}
