using FashionStore.Controllers;
using FashionStore.DTO;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IThanhToanRepository
    {
        Task<ResponseMessageResult> ThanhToanCODAsync(ThanhToanCODRequest request);
        Task<ResponseMessageResult> TaoDonHangKhiVNPAYThanhCongAsync(string maDonHang,int maDiaChi, int maKhachHang, string? maVoucher = null);
    }
}
