using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface ISanPhamRepository
    {
        Task<ResponseMessageResult> GetAllAsync();
        Task<ResponseMessageResult> GetByIdAsync(string id);
        Task<ResponseMessageResult> CreateAsync(SanPham sp, List<HinhAnhDTO>? hinhAnhs, List<SanPhamBienTheDTO>? bienThes = null);
        Task<ResponseMessageResult> UpdateAsync(string maSanPham, SanPhamDTO dto);
        Task<ResponseMessageResult> DeleteAsync(string id);
        Task<ResponseMessageResult> ToggleStatusAsync(string id);

        // Cập nhật riêng từng phần (PATCH)
        Task<ResponseMessageResult> PatchAsync(string id, SanPhamDTO dto);

        // Quản lý hình ảnh
        Task<ResponseMessageResult> AddImageAsync(HinhAnhSanPham img);
        Task<ResponseMessageResult> DeleteImageAsync(int id);

        // Phương thức mới: Quản lý biến thể
        Task<ResponseMessageResult> CreateBienTheAsync(string maSanPham, SanPhamBienTheDTO dto);

        //  Danh mục + Biến thể + Hình ảnh
        Task<ResponseMessageResult> TimKiemSanPhamAsync(SanPhamFilterDTO dto);
    }
}
