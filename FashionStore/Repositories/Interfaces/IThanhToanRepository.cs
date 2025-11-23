using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IThanhToanRepository
    {
        Task<ResponseMessageResult> ThanhToanCODAsync(int maKhachHang, string? maVoucher = null);
        Task<ResponseMessageResult> TaoDonHangKhiVNPAYThanhCongAsync(string maDonHang, int maKhachHang, string? maVoucher = null);
    }
}
