using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepo;

        public SanPhamController(ISanPhamRepository sanPhamRepo)
        {
            _sanPhamRepo = sanPhamRepo;
        }

        //======== GET: api/SanPham ========
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sanPhamRepo.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        //======== GET: api/SanPham/{maSanPham} ========
        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetById(string maSanPham)
        {
            var result = await _sanPhamRepo.GetByIdAsync(maSanPham);
            return StatusCode(result.StatusCode, result);
        }

        //======== POST: api/SanPham ========
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Ten_SanPham) || string.IsNullOrWhiteSpace(dto.Ma_DanhMuc))
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Dữ liệu sản phẩm không hợp lệ.", StatusCode = 400 });

            // Tạo SanPham từ DTO (repository sẽ sinh Ma_SanPham)
            var sp = new SanPham
            {
                Ma_SanPham = Guid.NewGuid().ToString("N").ToUpper(), // Set tạm để tránh lỗi required
                Ten_SanPham = dto.Ten_SanPham,
                Ma_DanhMuc = dto.Ma_DanhMuc,
                Mo_Ta = dto.Mo_Ta,
                Trang_Thai = dto.Trang_Thai
            };

            var result = await _sanPhamRepo.CreateAsync(sp, dto.BienThes);
            return StatusCode(result.StatusCode, result);
        }

        //======== PUT: api/SanPham/{maSanPham} ======== 
        [HttpPut("{maSanPham}")]
        public async Task<IActionResult> Update(string maSanPham, [FromBody] SanPhamDTO dto)
        {
            if (dto == null)
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Dữ liệu sản phẩm không hợp lệ.", StatusCode = 400 });

            var result = await _sanPhamRepo.UpdateAsync(maSanPham, dto);
            return StatusCode(result.StatusCode, result);
        }

        //======== DELETE: api/SanPham/{maSanPham} ========
        [HttpDelete("{maSanPham}")]
        public async Task<IActionResult> Delete(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Mã sản phẩm không hợp lệ.", StatusCode = 400 });

            var result = await _sanPhamRepo.DeleteAsync(maSanPham);
            return StatusCode(result.StatusCode, result);
        }

        ////======== POST: api/SanPham/{maSanPham}/images ========
        //[HttpPost("{maSanPham}/images")]
        //public async Task<IActionResult> AddImage(string maSanPham, [FromBody] HinhAnhDTO dto)
        //{
        //    if (dto == null || string.IsNullOrWhiteSpace(dto.DuongDan))
        //        return BadRequest(new ResponseMessageResult { Success = false, Message = "Đường dẫn hình ảnh không hợp lệ.", StatusCode = 400 });

        //    var img = new HinhAnhSanPham
        //    {
        //        Ma_SanPham = maSanPham,
        //        DuongDan = dto.DuongDan.Trim()
        //    };

        //    var result = await _sanPhamRepo.AddImageAsync(img);
        //    return StatusCode(result.StatusCode, result);
        //}

        ////======== DELETE: api/SanPham/images/{id} ========
        //[HttpDelete("images/{id}")]
        //public async Task<IActionResult> DeleteImage(int id)
        //{
        //    var result = await _sanPhamRepo.DeleteImageAsync(id);
        //    return StatusCode(result.StatusCode, result);
        //}

        //======== PATCH: api/SanPham/{maSanPham} ========
        [HttpPatch("{maSanPham}")]
        public async Task<IActionResult> Patch(string maSanPham, [FromBody] SanPhamDTO dto)
        {
            if (dto == null)
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Dữ liệu gửi lên không hợp lệ.", StatusCode = 400 });

            var result = await _sanPhamRepo.PatchAsync(maSanPham, dto);
            return StatusCode(result.StatusCode, result);
        }

        //======== PATCH: api/SanPham/toggle-status/{maSanPham} ========
        [HttpPatch("toggle-status/{maSanPham}")]
        public async Task<IActionResult> ToggleStatus(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Mã sản phẩm không hợp lệ.", StatusCode = 400 });

            var result = await _sanPhamRepo.ToggleStatusAsync(maSanPham);
            return StatusCode(result.StatusCode, result);
        }

        //======== POST: api/SanPham/{maSanPham}/bien-the ========
        [HttpPost("{maSanPham}/bien-the")]
        public async Task<IActionResult> CreateBienThe(string maSanPham, [FromBody] SanPhamBienTheDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Mau_Sac) || string.IsNullOrWhiteSpace(dto.Kich_Thuoc))
                return BadRequest(new ResponseMessageResult { Success = false, Message = "Dữ liệu biến thể không hợp lệ.", StatusCode = 400 });

            var result = await _sanPhamRepo.CreateBienTheAsync(maSanPham, dto);
            return StatusCode(result.StatusCode, result);
        }

        //  ===== GET: api/SanPham/tim-kiem
        [HttpGet("tim-kiem")]
        public async Task<IActionResult> TimKiem([FromQuery] SanPhamFilterDTO dto)
        {
            var result = await _sanPhamRepo.TimKiemSanPhamAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

    }
}
