using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Implementations;
using FashionStore.Repositories.Interfaces;
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
            var list = await _sanPhamRepo.GetAllAsync();
            return Ok(list);
        }

        //======== GET: api/SanPham/{maSanPham} ========
        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetById(string maSanPham)
        {
            var sp = await _sanPhamRepo.GetByIdAsync(maSanPham);
            if (sp == null) return NotFound("Không tìm thấy sản phẩm!");

            return Ok(sp);
        }

        //======== POST: api/SanPham ========
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            var sp = new SanPham
            {
                Ma_SanPham = Guid.NewGuid().ToString("N").ToUpper(),
                Ten_SanPham = dto.Ten_SanPham,
                Ma_DanhMuc = dto.Ma_DanhMuc,
                Mo_Ta = dto.Mo_Ta,
                Gia = dto.Gia,
                Gia_Giam = dto.Gia_Giam,
                So_Luong = dto.So_Luong,
                Mau_Sac = dto.Mau_Sac,
                Kich_Thuoc = dto.Kich_Thuoc,
                Trang_Thai = dto.Trang_Thai
            };

            var result = await _sanPhamRepo.CreateAsync(sp, dto.HinhAnhs);

            if (!result)
                return StatusCode(500, "Không thể tạo sản phẩm");

            return Ok(new { message = "Tạo sản phẩm thành công", maSanPham = sp.Ma_SanPham });
        }

        //======== PUT: api/SanPham/{maSanPham} ======== 
        [HttpPut("{maSanPham}")]
        public async Task<IActionResult> Update(string maSanPham, [FromBody] SanPhamDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu sản phẩm không hợp lệ.");

            try
            {
                bool result = await _sanPhamRepo.UpdateAsync(maSanPham, dto);

                if (!result)
                    return NotFound($"Không tìm thấy sản phẩm có mã {maSanPham}.");

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật sản phẩm thành công.",
                    data = dto
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi cập nhật sản phẩm: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi cập nhật sản phẩm.",
                    error = ex.Message
                });
            }
        }

        //======== DELETE: api/SanPham/{maSanPham} ========
        [HttpDelete("{maSanPham}")]
        public async Task<IActionResult> Delete(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                return BadRequest("Mã sản phẩm không hợp lệ.");

            try
            {
                bool result = await _sanPhamRepo.DeleteAsync(maSanPham);

                if (!result)
                    return NotFound($"Không tìm thấy sản phẩm có mã {maSanPham}.");

                return Ok(new
                {
                    success = true,
                    message = "Xóa sản phẩm thành công."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi xóa sản phẩm: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi xóa sản phẩm.",
                    error = ex.Message
                });
            }
        }

        //======== PUT: api/SanPham/{maSanPham}/images ========
        [HttpPost("{maSanPham}/images")]
        public async Task<IActionResult> AddImage(string maSanPham, [FromBody] HinhAnhDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.DuongDan))
                return BadRequest("Đường dẫn hình ảnh không hợp lệ.");

            try
            {
                var img = new HinhAnhSanPham
                {
                    Ma_SanPham = maSanPham,
                    DuongDan = dto.DuongDan.Trim()
                };

                bool result = await _sanPhamRepo.AddImageAsync(img);

                if (!result)
                    return NotFound($"Không tìm thấy sản phẩm có mã {maSanPham}.");

                return Ok(new
                {
                    success = true,
                    message = "Thêm hình ảnh thành công.",
                    data = img
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi thêm hình ảnh: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi thêm hình ảnh.",
                    error = ex.Message
                });
            }
        }

        //======== DELETE: api/SanPham/images/{id} ========
        [HttpDelete("images/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                bool result = await _sanPhamRepo.DeleteImageAsync(id);

                if (!result)
                    return NotFound($"Không tìm thấy hình ảnh với ID {id}.");

                return Ok(new
                {
                    success = true,
                    message = "Xóa hình ảnh thành công.",
                    imageId = id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi xóa hình ảnh: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi xóa hình ảnh.",
                    error = ex.Message
                });
            }
        }

        //======== Patch: api/SanPham/{maSanPham} ========
        [HttpPatch("{maSanPham}")]
        public async Task<IActionResult> Patch(string maSanPham, [FromBody] SanPhamDTO dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Dữ liệu gửi lên không hợp lệ." });

            try
            {
                bool result = await _sanPhamRepo.PatchAsync(maSanPham, dto);

                if (!result)
                    return NotFound(new { success = false, message = $"Không tìm thấy sản phẩm có mã {maSanPham}." });

                return Ok(new { success = true, message = "Cập nhật sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi cập nhật sản phẩm: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi cập nhật sản phẩm.",
                    error = ex.Message
                });
            }
        }

        //======== Patch: api/SanPham/toggle-status/{maSanPham} ========
        [HttpPatch("toggle-status/{maSanPham}")]
        public async Task<IActionResult> ToggleStatus(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                return BadRequest("Mã sản phẩm không hợp lệ.");

            try
            {
                bool result = await _sanPhamRepo.ToggleStatusAsync(maSanPham);

                if (!result)
                    return NotFound($"Không tìm thấy sản phẩm có mã {maSanPham}.");

                return Ok(new
                {
                    success = true,
                    message = "Thay đổi trạng thái sản phẩm thành công."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi đổi trạng thái sản phẩm: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi đổi trạng thái sản phẩm.",
                    error = ex.Message
                });
            }
        }


    }
}
