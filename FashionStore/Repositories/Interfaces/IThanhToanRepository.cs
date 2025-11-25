using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IThanhToanRepository
    {
        Task<ResponseMessageResult> ThanhToanCODAsync(int maKhachHang, int maDiaChi, string? maVoucher = null);
        Task<ResponseMessageResult> TaoDonHangKhiVNPAYThanhCongAsync(string maDonHang,int maDiaChi, int maKhachHang, string? maVoucher = null);
    }
}
