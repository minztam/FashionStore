using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangRepository _khachHangRepo;
        private readonly ITaiKhoanRepository _taiKhoanRepo;
        public KhachHangController(IKhachHangRepository khRepo, ITaiKhoanRepository taiKhoanRepo)
        {
            _khachHangRepo = khRepo;
            _taiKhoanRepo = taiKhoanRepo;
        }

        // GET: api/KhachHang
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var khList = await _khachHangRepo.GetAllAsync();

            var result = khList.Select(kh => new
            {
                MaKhachHang = kh.Ma_KhachHang,
                HoTen = kh.HoTen,
                SoDienThoai = kh.SoDienThoai,
                DiaChi = kh.DiaChi,
                HinhAnh = kh.Hinh_Anh,
                TaiKhoan = kh.TaiKhoan == null ? null : new
                {
                    MaTaiKhoan = kh.TaiKhoan.Ma_TaiKhoan,
                    TenDangNhap = kh.TaiKhoan.Ten_DangNhap,
                    Email = kh.TaiKhoan.Email,
                    VaiTro = kh.TaiKhoan.VaiTro?.Ten_VaiTro
                }
            });

            return Ok(result);
        }

        // GET: api/KhachHang/5
        [HttpGet("{maKhachHang}")]
        public async Task<IActionResult> GetById(int maKhachHang)
        {
            var kh = await _khachHangRepo.GetByIdAsync(maKhachHang);
            if (kh == null)
                return NotFound($"Không tìm thấy khách hàng với mã = {maKhachHang}");

            return Ok(new
            {
                MaKhachHang = kh.Ma_KhachHang,
                HoTen = kh.HoTen,
                SoDienThoai = kh.SoDienThoai,
                DiaChi = kh.DiaChi,
                HinhAnh = kh.Hinh_Anh,
                TaiKhoan = new
                {
                    MaTaiKhoan = kh.TaiKhoan?.Ma_TaiKhoan,
                    TenDangNhap = kh.TaiKhoan?.Ten_DangNhap,
                    Email = kh.TaiKhoan?.Email,
                    VaiTro = kh.TaiKhoan?.VaiTro?.Ten_VaiTro
                }
            });
        }

        // POST: api/KhachHang
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachHangDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

            // Kiểm tra tồn tại tài khoản
            var taiKhoan = await _taiKhoanRepo.GetByIdAsync(dto.Ma_TaiKhoan);
            if (taiKhoan == null) return BadRequest("Tài khoản liên kết không tồn tại.");

            var kh = new KhachHang
            {
                HoTen = dto.Ho_Ten,
                SoDienThoai = dto.So_Dien_Thoai,
                DiaChi = dto.Dia_Chi,
                Ma_TaiKhoan = dto.Ma_TaiKhoan // Gán tài khoản
            };

            var success = await _khachHangRepo.AddAsync(kh);
            if (!success) return StatusCode(500, "Tạo khách hàng thất bại.");

            return Ok(new
            {
                message = "Thêm khách hàng thành công!",
                //data = new
                //{
                //    MaKhachHang = kh.Ma_KhachHang,
                //    HoTen = kh.HoTen,
                //    SoDienThoai = kh.SoDienThoai,
                //    DiaChi = kh.DiaChi,
                //    HinhAnh = kh.Hinh_Anh,
                //    TaiKhoan = new
                //    {
                //        MaTaiKhoan = kh.TaiKhoan?.Ma_TaiKhoan,
                //        TenDangNhap = kh.TaiKhoan?.Ten_DangNhap,
                //        Email = kh.TaiKhoan?.Email,
                //        VaiTro = kh.TaiKhoan?.VaiTro?.Ten_VaiTro
                //    }
                //}
            });
        }

        // PUT: api/KhachHang/5
        [HttpPut("{maKhachHang}")]
        public async Task<IActionResult> Update(int maKhachHang, [FromBody] KhachHangDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

            var kh = await _khachHangRepo.GetByIdAsync(maKhachHang);
            if (kh == null) 
                return NotFound($"Không tìm thấy khách hàng với mã = {maKhachHang}");

            kh.HoTen = dto.Ho_Ten!;
            kh.SoDienThoai = dto.So_Dien_Thoai!;
            kh.DiaChi = dto.Dia_Chi!;

            var success = await _khachHangRepo.UpdateAsync(kh);
            if (!success) return StatusCode(500, "Cập nhật thất bại.");

            return Ok(new { message = "Cập nhật khách hàng thành công!"});
        }

        // PATCH: api/KhachHang/5
        [HttpPatch("{maKhachHang}")]
        public async Task<IActionResult> UpdatePartial(int maKhachHang, [FromBody] KhachHangDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

            var kh = await _khachHangRepo.GetByIdAsync(maKhachHang);
            if (kh == null) return NotFound($"Không tìm thấy khách hàng với mã = {maKhachHang}");

            if (!string.IsNullOrEmpty(dto.Ho_Ten)) kh.HoTen = dto.Ho_Ten;
            if (!string.IsNullOrEmpty(dto.So_Dien_Thoai)) kh.SoDienThoai = dto.So_Dien_Thoai;
            if (!string.IsNullOrEmpty(dto.Dia_Chi)) kh.DiaChi = dto.Dia_Chi;
            if (!string.IsNullOrEmpty(dto.Hinh_Anh)) kh.Hinh_Anh = dto.Hinh_Anh;

            // Cập nhật tài khoản nếu người dùng gửi khác với hiện tại
            if (dto.Ma_TaiKhoan != 0 && dto.Ma_TaiKhoan != kh.Ma_TaiKhoan)
            {
                var taiKhoan = await _taiKhoanRepo.GetByIdAsync(dto.Ma_TaiKhoan);
                if (taiKhoan == null)
                    return BadRequest("Tài khoản liên kết không tồn tại.");
                if (await _khachHangRepo.ExistsByAccountIdAsync(dto.Ma_TaiKhoan))
                    return BadRequest("Tài khoản này đã được gán cho nhân viên khác.");

                kh.Ma_TaiKhoan = dto.Ma_TaiKhoan;
            }

            var success = await _khachHangRepo.UpdateAsync(kh);
            if (!success) return StatusCode(500, "Cập nhật thất bại.");

            return Ok(new { message = "Cập nhật khách hàng thành công!" });
        }

        // DELETE: api/KhachHang/5
        [HttpDelete("{maKhachHang}")]
        public async Task<IActionResult> Delete(int maKhachHang)
        {
            var kh = await _khachHangRepo.GetByIdAsync(maKhachHang);
            if (kh == null) 
                return NotFound($"Không tìm thấy khách hàng với mã = {maKhachHang}");

            var success = await _khachHangRepo.DeleteAsync(maKhachHang);
            if (!success) return StatusCode(500, "Xóa khách hàng thất bại.");

            return Ok(new { message = "Xóa khách hàng thành công!" });
        }
    }
}
