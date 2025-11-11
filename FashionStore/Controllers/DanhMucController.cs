using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly IDanhMucRepository _dmRepo;
        public DanhMucController(IDanhMucRepository repo)
        {
            _dmRepo = repo;
        }

        // GET: api/DanhMuc
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _dmRepo.GetAllAsync();
            return Ok(data);
        }

        // GET: api/DanhMuc/{id}
        [HttpGet("{maDanhMuc}")]
        public async Task<IActionResult> GetById(string maDanhMuc)
        {
            var dm = await _dmRepo.GetByIdAsync(maDanhMuc);
            if (dm == null) return NotFound();
            return Ok(dm);
        }

        // Tìm + gợi ý theo TÊN
        //api/DanhMuc/{tenDanhMuc}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DanhMucDTO>>> Search(string? keyword)
        {
            var result = await _dmRepo.SearchAsync(keyword);
            return Ok(result);
        }

        // GET: api/DanhMuc/tree
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var data = await _dmRepo.GetTreeAsync();
            return Ok(data);
        }

        // POST: api/DanhMuc
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DanhMucDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var dm = new DanhMuc
            {
                Ma_DanhMuc = dto.Ma_DanhMuc.Trim().ToUpper(),
                Ten_DanhMuc = dto.Ten_DanhMuc.Trim(),
                Ma_DanhMucCha = string.IsNullOrWhiteSpace(dto.Ma_DanhMucCha) ? null : dto.Ma_DanhMucCha.Trim().ToUpper(),
                Trang_Thai = dto.Trang_Thai
            };

            var success = await _dmRepo.AddAsync(dm);
            if (!success)
                return BadRequest("Không thể tạo danh mục. Kiểm tra trùng mã hoặc danh mục cha không tồn tại.");

            return Ok(new { message = "Thêm danh mục thành công!", data = dm });
        }

        // PUT: api/DanhMuc/{maDanhMuc}
        [HttpPut("{maDanhMuc}")]
        public async Task<IActionResult> Update(string maDanhMuc, [FromBody] DanhMucDTO dto)
        {
            if (dto == null || maDanhMuc != dto.Ma_DanhMuc)
                return BadRequest("Dữ liệu không hợp lệ.");

            var dm = new DanhMuc
            {
                Ma_DanhMuc = maDanhMuc.Trim().ToUpper(),
                Ten_DanhMuc = dto.Ten_DanhMuc,
                Ma_DanhMucCha = dto.Ma_DanhMucCha,
                Trang_Thai = dto.Trang_Thai
            };

            var success = await _dmRepo.UpdateAsync(dm);
            if (!success)
                return BadRequest("Không thể cập nhật danh mục. Kiểm tra dữ liệu.");

            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }

        // DELETE: api/DanhMuc/{maDanhMuc}
        [HttpDelete("{maDanhMuc}")]
        public async Task<IActionResult> Delete(string maDanhMuc)
        {
            var result = await _dmRepo.DeleteAsync(maDanhMuc);

            if (!result)
                return BadRequest(new { message = "Không thể xóa danh mục (có thể tồn tại danh mục con hoặc sản phẩm liên quan)." });

            return Ok(new { message = "Xóa danh mục thành công!" });
        }

        // PATCH: api/DanhMuc/toggle/{id}
        [HttpPut("toggle/{maDanhMuc}")]
        public async Task<IActionResult> ToggleStatus(string maDanhMuc)
        {
            var result = await _dmRepo.ToggleStatusAsync(maDanhMuc);

            if (!result)
                return BadRequest(new { message = "Không thể thay đổi trạng thái danh mục." });

            return Ok(new { message = "Thay đổi trạng thái danh mục thành công!" });
        }
    }
}
