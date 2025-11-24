using FashionStore.DTO;
using FashionStore.Repositories.Interfaces;
using FashionStore.Services;
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
        private readonly EmailService _emailService;

        public AuthController(ITaiKhoanRepository taiKhoanRepo, JwtService jwtService, EmailService emailService)
        {
            _taiKhoanRepo = taiKhoanRepo;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Ten_DangNhap) || string.IsNullOrWhiteSpace(dto.Mat_Khau))
                return BadRequest("Tên đăng nhập và mật khẩu không được để trống.");

            var taiKhoan = await _taiKhoanRepo.LoginAsync(dto.Ten_DangNhap, dto.Mat_Khau);
            if (taiKhoan == null)
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu!");

            var role = taiKhoan.VaiTro?.Ten_VaiTro ?? "KhachHang"; // mặc định KhachHang nếu null
            var token = _jwtService.GenerateToken(taiKhoan.Ten_DangNhap, role);

            // Response chỉ phân biệt Admin và KhachHang
            object userResponse = role == "Admin"
                ? new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    VaiTro = role,
                    Trang = "/admin" // ví dụ link trang admin
                }
                : new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    VaiTro = "KhachHang",
                    Trang = "/" // ví dụ trang chính khách hàng
                };

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token,
                user = userResponse
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var taiKhoan = await _taiKhoanRepo.RegisterCustomerAsync(dto);

            if (taiKhoan == null)
                return BadRequest("Tên đăng nhập hoặc email đã tồn tại!");

            // Gửi email chào mừng
            await _emailService.SendWelcomeEmailAsync(dto.Email, dto.Ten_DangNhap);

            // Tạo JWT token
            var token = _jwtService.GenerateToken(
                taiKhoan.Ten_DangNhap,
                "KhachHang"
            );

            return Ok(new
            {
                message = "Đăng ký thành công!",
                token,
                user = new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    taiKhoan.Email,
                    VaiTro = "KhachHang"
                }
            });
        }

        [HttpPost("SaleAssistanceRegistration")]
        public async Task<IActionResult> SaleAssistanceRegistration([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var taiKhoan = await _taiKhoanRepo.RegisterSaleAssistanceAsync(dto);

            if (taiKhoan == null)
                return BadRequest("Tên đăng nhập hoặc email đã tồn tại!");

            // Gửi email chào mừng
            await _emailService.SendWelcomeEmailAsync(dto.Email, dto.Ten_DangNhap);

            // Tạo JWT token
            var token = _jwtService.GenerateToken(
                taiKhoan.Ten_DangNhap,
                "NhanVienBanHang"
            );

            return Ok(new
            {
                message = "Đăng ký thành công!",
                token,
                user = new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    taiKhoan.Email,
                    VaiTro = "NhanVienBanHang"
                }
            });
        }

        [HttpPost("WarehouseAssistanceRegistration")]
        public async Task<IActionResult> WarehouseAssistanceRegistration([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var taiKhoan = await _taiKhoanRepo.RegisterWarehouseAssistanceAsync(dto);

            if (taiKhoan == null)
                return BadRequest("Tên đăng nhập hoặc email đã tồn tại!");

            // Gửi email chào mừng
            await _emailService.SendWelcomeEmailAsync(dto.Email, dto.Ten_DangNhap);

            // Tạo JWT token
            var token = _jwtService.GenerateToken(
                taiKhoan.Ten_DangNhap,
                "NhanVienKho"
            );

            return Ok(new
            {
                message = "Đăng ký thành công!",
                token,
                user = new
                {
                    taiKhoan.Ma_TaiKhoan,
                    taiKhoan.Ten_DangNhap,
                    taiKhoan.Email,
                    VaiTro = "NhanVienKho"
                }
            });
        }


    }
}
