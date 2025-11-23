using FashionStore.DTO;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Repositories.Interfaces
{
    public interface IDonHangRepository
    {
        Task<ResponseMessageResult> GetAllDonHangAsync();
        Task<ResponseMessageResult> TaoDonHangAsync(TaoDonHangRequest request, DonHangDTO? responseDto = null);
        Task<ResponseMessageResult> GetChiTietDonHangAsync(string maDH);
        Task<ResponseMessageResult> ThongKeDonHangAsync(DateTime? fromDate, DateTime? toDate, string? groupBy);
        Task<ResponseMessageResult> GetDonHangByKhachHangAsync(int maKhachHang);
        Task<ResponseMessageResult> CapNhatTrangThaiAsync(string maDonHang, string trangThaiMoi);
        Task<string> GenerateInvoiceHtmlAsync(string maDonHang);
    }
}
