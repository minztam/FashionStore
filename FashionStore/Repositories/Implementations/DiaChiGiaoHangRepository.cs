using Azure;
using FashionStore.Data;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class DiaChiGiaoHangRepository (FashionStoreContext _context, ResponseMessageResult _response) : IDiaChiGiaoHangRepository
    {
     
        public async Task<ResponseMessageResult> GetAddress()
        {
            var address =await _context.DiaChiGiaoHangs.ToListAsync();
            if (address.Count == 0)
            {
                _response.SetCustom(true, null!, 200, null);
            }
            _response.SetSuccess("Lấy danh sách danh mục thành công", address);
            return _response;
           
           
        }
    }
}
