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
        public async Task<IActionResult> GetAll()
        {
            var data = await _dmRepo.GetAddress();
            return Ok(data);
        }
    }
}
