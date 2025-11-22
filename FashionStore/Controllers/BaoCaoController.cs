using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaoCaoController : ControllerBase
    {
        private readonly IBaoCaoRepository _baoCaoRepo;

        public BaoCaoController(IBaoCaoRepository baoCaoRepo)
        {
            _baoCaoRepo = baoCaoRepo;
        }

        [HttpGet("doanh-thu")]
        public async Task<IActionResult> ThongKeDoanhThu(DateTime? fromDate, DateTime? toDate, string? groupBy)
        {
            var result = await _baoCaoRepo.ThongKeDoanhThuAsync(fromDate, toDate, groupBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("san-pham-ban-chay")]
        public async Task<IActionResult> SanPhamBanChay(int top = 10)
        {
            var result = await _baoCaoRepo.SanPhamBanChayAsync(top);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("khach-hang-moi")]
        public async Task<IActionResult> KhachHangMoi(DateTime? fromDate, DateTime? toDate, string? groupBy)
        {
            var result = await _baoCaoRepo.KhachHangMoiAsync(fromDate, toDate, groupBy);
            return StatusCode(result.StatusCode, result);
        }
    }
}
