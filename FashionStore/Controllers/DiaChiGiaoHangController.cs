using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaChiGiaoHangController : ControllerBase
    {
        private readonly IDiaChiGiaoHangRepository _dmRepo;
        public DiaChiGiaoHangController(IDiaChiGiaoHangRepository repo)
        {
            _dmRepo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int maKhachHang)
        {
            var data = await _dmRepo.GetAddress(maKhachHang);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DiaChiGiaoHangDTO model)
        {
            var result = await _dmRepo.AddAddress(model);
            return Ok(result);
        }

        // PUT: api/DiaChiGiaoHang/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiaChiGiaoHangDTO model)
        {
            var result = await _dmRepo.UpdateAddress(id, model);
            return Ok(result);
        }

        // DELETE: api/DiaChiGiaoHang/5
        [HttpDelete("{maDiaChi}")]
        public async Task<IActionResult> Delete(int maDiaChi)
        {
            var result = await _dmRepo.DeleteAddress(maDiaChi);
            return Ok(result);
        }
    }
}
