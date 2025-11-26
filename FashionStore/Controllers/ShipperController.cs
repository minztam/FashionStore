using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [ApiController]
    [Route("api/shippers")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepo;

        public ShipperController(IShipperRepository shipperRepo)
        {
            _shipperRepo = shipperRepo;
        }

        // ==============================
        // GET: api/shippers
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _shipperRepo.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // ==============================
        // GET: api/shippers/{id}
        // ==============================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _shipperRepo.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ==============================
        // POST: api/shippers
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShipperRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepo.AddAsync(dto); // Repository nhận DTO
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/shippers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShipperRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shipperRepo.UpdateAsync(id, dto); // Repository nhận DTO
            return StatusCode(result.StatusCode, result);
        }

        // ==============================
        // DELETE: api/shippers/{id}
        // ==============================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _shipperRepo.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ==============================
        // PATCH: api/shippers/{id}/toggle-status
        // ==============================
        [HttpPatch("{id:int}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _shipperRepo.ToggleStatusAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
