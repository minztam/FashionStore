using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IGioHangRepository
    {
        Task<ResponseMessageResult> GetCartAsync(int maTaiKhoan);
        Task<ResponseMessageResult> AddToCartAsync(int maKhachHang, string maSanPham, int soLuong);
        Task<ResponseMessageResult> UpdateCartAsync(int maKhachHang, string maSanPham, int soLuong);
        Task<ResponseMessageResult> RemoveFromCartAsync(int maKhachHang, string maSanPham);
    }
}
