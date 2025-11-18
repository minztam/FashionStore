using FashionStore.DTO;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{maDH}")]
        public async Task<IActionResult> GetDonHang(string maDH)
        {
            var result = await _donHangRepo.GetDonHang(maDH);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [HttpPost("tao-don-hang")]
        public async Task<IActionResult> TaoDonHang([FromBody] DonHangDTO dto)
        {
            if (dto == null)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!"));

            var result = await _donHangRepo.TaoDonHangAsync(dto);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }
    }
}
