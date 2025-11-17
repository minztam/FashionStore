using FashionStore.DTO;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanRepository _taikhoanRepo;
        private readonly IVaiTroRepository _vaiTroRepo;
        private readonly EmailService _emailService;
        public TaiKhoanController(ITaiKhoanRepository taiKhoanRepo, IVaiTroRepository vaiTroRepo, EmailService emailService)
        {
            _taikhoanRepo = taiKhoanRepo;
            _vaiTroRepo = vaiTroRepo;
            _emailService = emailService;
        }

        // GET api/TaiKhoan
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _taikhoanRepo.GetAllAsync();
            return Ok(list);
        }

        // GET api/TaiKhoan/{maTaiKhoan}
        [HttpGet("{maTaiKhoan}")]
        public async Task<IActionResult> GetById(int maTaiKhoan)
        {
            var taiKhoan = await _taikhoanRepo.GetByIdAsync(maTaiKhoan);
            if (taiKhoan == null)
                return NotFound($"Không tìm thấy tài khoản với ID: {maTaiKhoan}");

            // Trả về dữ liệu (có thể là toàn bộ TaiKhoan hoặc chọn các trường cần thiết)
            return Ok(new
            {
                taiKhoan.Ma_TaiKhoan,
                taiKhoan.Ten_DangNhap,
                taiKhoan.Email,
                taiKhoan.Trang_Thai,
                taiKhoan.Da_XacThuc,
                VaiTro = new
                {
                    taiKhoan.VaiTro?.Ma_VaiTro,
                    taiKhoan.VaiTro?.Ten_VaiTro
                }
            });
        }

        // POST api/TaiKhoan
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaiKhoanDTO dto, [FromServices] EmailService emailService)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!"));

                // Gọi Repo – Repo tự kiểm tra trùng username, email, sinh KH hoặc NV
                var result = await _taikhoanRepo.AddAsync(
                    dto.Ten_DangNhap!,
                    dto.Mat_Khau!,
                    dto.Ma_VaiTro!,
                    dto.Email
                );

                // Nếu repo trả lỗi
                if (!result.Success)
                    return StatusCode(result.StatusCode, result);

                // Nếu tạo tài khoản KH → gửi email chào mừng
                bool isKhachHang = dto.Ma_VaiTro == "2222-2222-2222-2222";
                if (isKhachHang && !string.IsNullOrWhiteSpace(dto.Email))
                {
                    await emailService.SendWelcomeEmailAsync(dto.Email!, dto.Ten_DangNhap!);
                }

                // Trả về phản hồi thành công
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessageResult().SetFail("Lỗi server: " + ex.Message, 500));
            }
        }

        // PUT api/TaiKhoan/{maTaiKhoan}
        [HttpPut("{maTaiKhoan}")]
        public async Task<IActionResult> UpdateAccount(int maTaiKhoan, [FromBody] TaiKhoanDTO dto)
        {
            try
            {
                // Kiểm tra tồn tại tài khoản
                var taiKhoan = await _taikhoanRepo.GetByIdAsync(maTaiKhoan);
                if (taiKhoan == null)
                    return NotFound($"Không tìm thấy tài khoản với ID: {maTaiKhoan}");

                // Thực hiện cập nhật
                var result = await _taikhoanRepo.UpdateAsync(
                    maTaiKhoan,
                    dto.Ten_DangNhap,
                    dto.Mat_Khau,
                    dto.Ma_VaiTro,
                    dto.Email
                );

                if (!result)
                    return BadRequest("Cập nhật thất bại!");

                // Gửi email nếu vai trò là khách hàng và có email
                bool isKhachHang = dto.Ma_VaiTro == "2222-2222-2222-2222" || taiKhoan.Ma_VaiTro == "2222-2222-2222-2222";
                if (isKhachHang && !string.IsNullOrWhiteSpace(dto.Email))
                {
                    await _emailService.SendAccountUpdatedEmailAsync(dto.Email!, dto.Ten_DangNhap!);
                }

                return Ok("Cập nhật tài khoản thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        // PATCH api/TaiKhoan/{maTaiKhoan}
        [HttpPatch("{maTaiKhoan:int}")]
        public async Task<IActionResult> PatchAccount(int maTaiKhoan, [FromBody] TaiKhoanDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            try
            {
                var result = await _taikhoanRepo.UpdatePartialAsync(maTaiKhoan, dto);
                if (!result)
                    return BadRequest("Cập nhật tài khoản thất bại!");

                return Ok("Cập nhật tài khoản thành công!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        // DELETE api/TaiKhoan/{maTaiKhoan}
        [HttpDelete("{maTaiKhoan}")]
        public async Task<IActionResult> DeleteAccount(int maTaiKhoan)
        {
            try
            {
                var result = await _taikhoanRepo.DeleteAsync(maTaiKhoan);
                if (!result)
                    return NotFound($"Không tìm thấy tài khoản với ID: {maTaiKhoan} hoặc đã xóa trước đó.");

                return Ok("Xóa tài khoản thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        // PATCH: api/TaiKhoan/Toggle/{maTaiKhoan}
        [HttpPatch("Toggle/{maTaiKhoan}")]
        public async Task<IActionResult> ToggleTrangThai(int maTaiKhoan)
        {
            var tk = await _taikhoanRepo.GetByIdAsync(maTaiKhoan);
            if(tk == null)
                return BadRequest($"Không tìm thấy tài khoản với mã = {maTaiKhoan}");

            bool success = await _taikhoanRepo.ToggleStatusAsync(maTaiKhoan);
            if(!success)
                return StatusCode(500, "Cập nhật trạng thái thất bại.");

            var status = tk.Trang_Thai ? "Mở khóa" : "Khóa";
            return Ok(new
            {
                message = $"{status} tài khoản thành công!",
                data = new
                {
                    tk.Ma_TaiKhoan,
                    tk.Ten_DangNhap,
                    tk.Email,
                    TrangThai = tk.Trang_Thai ? "Hoạt động" : "Bị khóa"
                }
            });
        }

        [HttpPut("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO dto)
        {
            var existTaiKhoan = await _taikhoanRepo.GetByIdAsync(dto.Ma_TaiKhoan);
            if (existTaiKhoan == null) return NotFound("Tài khoản không tồn tại!");

            var existRole = await _vaiTroRepo.GetByIdAsync(dto.Ma_VaiTro);
            if (existRole == null) return BadRequest("Vai trò không hợp lệ!");

            var result = await _taikhoanRepo.AssignRoleAsync(dto.Ma_TaiKhoan, dto.Ma_VaiTro);
            if (!result) return BadRequest("Gán vai trò thất bại!");

            return Ok("Gán vai trò thành công!");
        }

    }
}
