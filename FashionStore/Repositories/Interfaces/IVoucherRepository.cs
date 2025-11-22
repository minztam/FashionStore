using FashionStore.DTO;
using FashionStore.Repositories.ResponseMessage;

namespace FashionStore.Repositories.Interfaces
{
    public interface IVoucherRepository
    {
        Task<ResponseMessageResult> GetByCodeAsync(string maVoucher);
        Task<ResponseMessageResult> GetAllAsync();
        Task<ResponseMessageResult> AddAsync(VoucherDTO voucherDto);
        Task<ResponseMessageResult> UpdateAsync(VoucherDTO voucherDto);
        Task<ResponseMessageResult> PatchAsync(string maVoucher, VoucherDTO patchDoc);
        Task<ResponseMessageResult> DeleteAsync(string maVoucher);
        Task<ResponseMessageResult> KiemTraVaTinhGiamGiaAsync(string maVoucher, decimal tongTien);
        Task<ResponseMessageResult> ApDungVoucherAsync(string maVoucher);
        Task<ResponseMessageResult> ApplyVoucherGioHangAsync(int maKhachHang, string maVoucher);
    }
}
