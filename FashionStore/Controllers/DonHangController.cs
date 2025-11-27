using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangRepository _donHangRepo;
        public DonHangController(IDonHangRepository donHangRepo)
        {
            _donHangRepo = donHangRepo;
        }

        [HttpGet("tat-ca")]
        public async Task<IActionResult> GetAllDonHang()
        {
            var result = await _donHangRepo.GetAllDonHangAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{maDH}")]
        public async Task<IActionResult> GetChiTietDonHangAsync(string maDH)
        {
            var result = await _donHangRepo.GetChiTietDonHangAsync(maDH);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [HttpPost("dat-hang")]
        public async Task<IActionResult> DatHang([FromBody] TaoDonHangRequest request)
        {
            if (request == null)
                return BadRequest(new ResponseMessageResult().SetFail("Yêu cầu không hợp lệ!", 400));

            var responseDto = new DonHangDTO(); // Frontend sẽ nhận được đầy đủ thông tin

            var result = await _donHangRepo.TaoDonHangAsync(request, responseDto);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            // Trả về 200 + dữ liệu đẹp lung linh
            return Ok(new ResponseMessageResult().SetSuccess("Đặt hàng thành công! Shipper đang chạy tới lấy hàng đây!", responseDto));
        }

        [HttpGet("thong-ke")]
        public async Task<IActionResult> ThongKe([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? groupBy)
        {
            var result = await _donHangRepo.ThongKeDonHangAsync(fromDate, toDate, groupBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("khach-hang/{maKhachHang}")]
        public async Task<IActionResult> GetDonHangKhachHang(int maKhachHang)
        {
            var result = await _donHangRepo.GetDonHangByKhachHangAsync(maKhachHang);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("shipper/{maShipper}")]
        public async Task<IActionResult> GetDonHangShipper(int maShipper)
        {
            var result = await _donHangRepo.GetDonHangByShipperAsync(maShipper);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("cap-nhat-trang-thai")]
        public async Task<IActionResult> CapNhatTrangThai([FromQuery] string maDonHang, [FromQuery] string trangThaiMoi)
        {
            var result = await _donHangRepo.CapNhatTrangThaiAsync(maDonHang, trangThaiMoi);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("in-hoa-don/{maDonHang}")]
        public async Task<IActionResult> InHoaDon(string maDonHang)
        {
            var html = await _donHangRepo.GenerateInvoiceHtmlAsync(maDonHang);
            return Content(html, "text/html", Encoding.UTF8);
        }
        [HttpPost("gan-shipper-random/{maDonHang}")]
        public async Task<IActionResult> GanDonHangChoShipperRandom([FromRoute] string maDonHang)
        {
            if (string.IsNullOrEmpty(maDonHang))
                return BadRequest(new ResponseMessageResult().SetFail("Mã đơn hàng không hợp lệ!", 400));

            var result = await _donHangRepo.GanDonHangChoShipperAsync(maDonHang);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }


    }
}
