using FashionStore.Models;

namespace FashionStore.Repositories.Interfaces
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang?> GetByIdAsync(int id);
        Task<bool> AddAsync(KhachHang kh);
        Task<bool> UpdateAsync(KhachHang kh);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByAccountIdAsync(int ma_TaiKhoan);
    }
}
