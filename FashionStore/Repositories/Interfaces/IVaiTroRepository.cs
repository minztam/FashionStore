using FashionStore.Models;

namespace FashionStore.Repositories.Interfaces
{
    public interface IVaiTroRepository
    {
        Task<IEnumerable<VaiTro>> GetAllAsync();
        Task<VaiTro?> GetByIdAsync(string maVaiTro);
        Task<bool> AddAsync(VaiTro entity);
        Task<bool> UpdateAsync(string id, string teVaiTroMoi);
        Task<bool> DeleteAsync(string maVaiTro);
    }
}
