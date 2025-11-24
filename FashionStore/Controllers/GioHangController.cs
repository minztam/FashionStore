using FashionStore.DTO;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IGioHangRepository _gioHangRepo;
        public GioHangController(IGioHangRepository gioHangRepo)
        {
            _gioHangRepo = gioHangRepo;
        }

        [HttpGet("view")]
        public async Task<IActionResult> GetCart(int maTaiKhoan)
        {
            var result = await _gioHangRepo.GetCartAsync(maTaiKhoan);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{maKhachHang}/add")]
        public async Task<IActionResult> AddToCart(int maKhachHang,string maSanPham,int soLuong,int maBienThe)
        {

            var result = await _gioHangRepo.AddToCartAsync(maKhachHang,maSanPham, soLuong, maBienThe);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{maKhachHang}/update")]
        public async Task<IActionResult> UpdateCart(int maKhachHang, [FromBody] ChiTietGioHangDTO dto)
        {
            if (dto == null)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu gửi lên không hợp lệ!"));

            var result = await _gioHangRepo.UpdateCartAsync(maKhachHang, dto.Ma_SanPham, dto.So_Luong, dto.Ma_BienThe);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{maKhachHang}/remove")]
        public async Task<IActionResult> RemoveFromCart(int maKhachHang, [FromBody] ChiTietGioHangDTO dto)
        {
            if (dto == null)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu gửi lên không hợp lệ!"));

            var result = await _gioHangRepo.RemoveFromCartAsync(maKhachHang, dto.Ma_SanPham, dto.Ma_BienThe);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(result.StatusCode, result);
        }

    }
}
