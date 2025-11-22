using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IBaoCaoRepository
    {
        Task<ResponseMessageResult> ThongKeDoanhThuAsync(DateTime? fromDate, DateTime? toDate, string? groupBy);
        Task<ResponseMessageResult> SanPhamBanChayAsync(int top = 10);
        Task<ResponseMessageResult> KhachHangMoiAsync(DateTime? fromDate, DateTime? toDate, string? groupBy);
    }
}
