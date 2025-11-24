using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface ITaiKhoanRepository
    {
        Task<ResponseMessageResult> GetAllAsync();
        Task<TaiKhoan?> GetByIdAsync(int maTaiKhoan);
        Task<ResponseMessageResult> AddAsync(string tenDangNhap, string matKhau, string maVaiTro, string? email);
        Task<bool> UpdateAsync(int maTaiKhoan, string? tenDangNhap, string? matKhau, string? maVaiTro, string? email);
        Task<bool> UpdatePartialAsync(int maTaiKhoan, TaiKhoanDTO dto);
        Task<bool> DeleteAsync(int maTaiKhoan);
        Task<TaiKhoan?> LoginAsync(string username, string password);
        Task<TaiKhoan?> RegisterCustomerAsync(RegisterDTO dto);
      
        Task<TaiKhoan?> RegisterSaleAssistanceAsync(RegisterDTO dto);
        Task<TaiKhoan?> RegisterWarehouseAssistanceAsync(RegisterDTO dto);

        Task<bool> AssignRoleAsync(int taiKhoanId, string roleId);
        Task<bool> ExistsByUsernameAsync(string ten_DangNhap);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ToggleStatusAsync(int maTaiKhoan);
    }
}
