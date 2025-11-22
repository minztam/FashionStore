using Azure;
using FashionStore.DTO;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepo;
        public VoucherController(IVoucherRepository voucherRepo)
        {
            _voucherRepo = voucherRepo;
        }

        // GET: api/voucher - Lấy danh sách voucher
        [HttpGet]
        public async Task<IActionResult> GetVouchers()
        {
            var result = await _voucherRepo.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/voucher/{maVoucher} - Lấy voucher theo mã
        [HttpGet("{maVoucher}")]
        public async Task<IActionResult> GetVoucherId(string maVoucher)
        {
            var result = await _voucherRepo.GetByCodeAsync(maVoucher);
            if (result.Success)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }

        // POST: api/voucher - Tạo voucher mới
        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liễu không hợp lệ"));
            }

            var result = await _voucherRepo.AddAsync(voucherDTO);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/voucher - Cập nhật voucher
        [HttpPut]
        public async Task<IActionResult> UpdateVoucher([FromBody] VoucherDTO voucherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ"));
            }
            var result = await _voucherRepo.UpdateAsync(voucherDto);
            return StatusCode(result.StatusCode, result);
        }

        // PATCH: api/voucher - cập nhật voucher
        [HttpPatch("{maVoucher}")]
        public async Task<IActionResult> Patch(string maVoucher, [FromBody] VoucherDTO patchDto)
        {
            if (patchDto == null)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không được để trống!", 400));

            var result = await _voucherRepo.PatchAsync(maVoucher,patchDto);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/voucher - xóa voucher
        [HttpDelete("{maVoucher}")]
        public async Task<IActionResult> Delete(string maVoucher)
        {
            if (string.IsNullOrWhiteSpace(maVoucher))
                return BadRequest(new ResponseMessageResult().SetFail("Mã voucher không được để trống!", 400));

            var result = await _voucherRepo.DeleteAsync(maVoucher);

            return StatusCode(result.StatusCode, result);
        }

        // POST : api/voucher -kiểm tra và tính giá giảm
        [HttpPost("check-discount")]
        public async Task<IActionResult> CheckDiscount([FromBody] CheckDiscountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!", 400));

            var result = await _voucherRepo.KiemTraVaTinhGiamGiaAsync(request.Ma_Voucher, request.TongTien);

            return StatusCode(result.StatusCode, result);
        }

        //
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ"));
            }
            var result = await _voucherRepo.ApDungVoucherAsync(request.Ma_Voucher);
            return StatusCode(result.StatusCode, result);
        }

        //
        [HttpPost("applyGioHang")]
        public async Task<IActionResult> ApplyVoucherGioHang([FromBody] ApplyVoucherRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!", 400));

            var result = await _voucherRepo.ApplyVoucherGioHangAsync(request.MaKhachHang, request.Ma_Voucher);
            return StatusCode(result.StatusCode, result);
        }
    }

    // Request models
    public class CheckDiscountRequest
    {
        [Required]
        public required string Ma_Voucher { get; set; }
        [Required]
        public decimal TongTien { get; set; }
    }

    public class ApplyVoucherRequest
    {
        [Required]
        public required string Ma_Voucher { get; set; }
        public int MaKhachHang { get; set; }
    }
}
