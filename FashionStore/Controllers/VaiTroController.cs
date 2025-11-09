using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroRepository _vaiTroRepo;

        public VaiTroController(IVaiTroRepository vaiTroRepo)
        {
            _vaiTroRepo = vaiTroRepo;
        }

        // GET: api/VaiTro
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _vaiTroRepo.GetAllAsync();
            return Ok(data);
        }

        // GET: api/VaiTro/{id}
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var vaiTro = await _vaiTroRepo.GetByIdAsync(id);
            if (vaiTro == null)
                return NotFound("Không tìm thấy vai trò");

            return Ok(vaiTro);
        }

        // POST: api/VaiTro
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VaiTroDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var vaiTro = new VaiTro
            {
                Ma_VaiTro = dto.Ma_VaiTro,
                Ten_VaiTro = dto.Ten_VaiTro
            };

            var result = await _vaiTroRepo.AddAsync(vaiTro);

            if (!result)
                return BadRequest("Thêm vai trò thất bại!");

            return Ok(new { Message = "Thêm vai trò thành công!", VaiTro = vaiTro });
        }

        // PUT: api/VaiTro
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] VaiTroDTO dto)
        {
            try
            {
                var result = await _vaiTroRepo.UpdateAsync(id, dto.Ten_VaiTro);
                if (!result) return BadRequest("Cập nhật thất bại!");
                return Ok("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/VaiTro/{id}
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _vaiTroRepo.DeleteAsync(id);

            if (!result)
                return NotFound($"Vai trò với id {id} không tồn tại hoặc xóa thất bại!");

            return Ok("Xóa vai trò thành công!");
        }
    }
}
