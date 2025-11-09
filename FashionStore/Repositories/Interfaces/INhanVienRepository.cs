using FashionStore.Models;

namespace FashionStore.Repositories.Interfaces
{
    public interface INhanVienRepository
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();
        Task<NhanVien?> GetByIdAsync(int id);
        Task<bool> AddAsync(NhanVien nv);
        Task<bool> UpdateAsync(NhanVien nv);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByAccountIdAsync(int maTaiKhoan);
    }
}
