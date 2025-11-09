using FashionStore.DTO;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITaiKhoanRepository _taiKhoanRepo;
        private readonly JwtService _jwtService;

        public AuthController(ITaiKhoanRepository taiKhoanRepo, JwtService jwtService)
        {
            _taiKhoanRepo = taiKhoanRepo;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromServices] JwtService jwtService)
        {
            var taiKhoan = await _taiKhoanRepo.LoginAsync(dto.Ten_DangNhap, dto.Mat_Khau);
            if (taiKhoan == null)
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!");

            var token = jwtService.GenerateToken(taiKhoan.Ten_DangNhap, taiKhoan.VaiTro!.Ten_VaiTro);

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token,
                user = new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    VaiTro = taiKhoan.VaiTro.Ten_VaiTro
                }
            });
        }
    }
}
