using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IGioHangRepository
    {
        Task<ResponseMessageResult> GetCartAsync(int maTaiKhoan);
        Task<ResponseMessageResult> AddToCartAsync(int maKhachHang, string maSanPham, int soLuong, int maBienThe);
        Task<ResponseMessageResult> UpdateCartAsync(int maKhachHang, string maSanPham, int soLuong, int maBienThe);
        Task<ResponseMessageResult> RemoveFromCartAsync(int maKhachHang, string maSanPham, int maBienThe);
    }
}
