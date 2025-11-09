using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienRepository _nhanVienRepo;
        private readonly ITaiKhoanRepository _taiKhoanRepo;

        public NhanVienController(INhanVienRepository nhanVienRepo, ITaiKhoanRepository taiKhoanRepo)
        {
            _nhanVienRepo = nhanVienRepo;
            _taiKhoanRepo = taiKhoanRepo;
        }

        // Lấy tất cả thông tin nhân viên
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _nhanVienRepo.GetAllAsync();
            return Ok(list);
        }

        // GET: Lấy thông tin nhân viên theo {maNhanVien}
        [HttpGet("{maNhanVien}")]
        public async Task<IActionResult> GetById(int maNhanVien)
        {
            var nv = await _nhanVienRepo.GetByIdAsync(maNhanVien);
            if (nv == null) return BadRequest($"Không tìm thấy nhân viên với mã tài khoản = {maNhanVien}");
            return Ok(new
            {
                MaNhanVien = nv.Ma_NhanVien,
                HoTen = nv.HoTen,
                SoDienThoai = nv.SoDienThoai,
                DiaChi = nv.DiaChi,
                HinhAnh = nv.Hinh_Anh,
                TaiKhoan = new
                {
                    nv.TaiKhoan?.Ma_TaiKhoan,
                    nv.TaiKhoan?.Email,
                    nv.TaiKhoan?.Ten_DangNhap,
                    VaiTro = new
                    { nv.TaiKhoan?.VaiTro?.Ten_VaiTro }
                }
            });
        }

        // POST: Thêm nhân viên mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

            // Kiểm tra tồn tại TaiKhoan
            var taiKhoan = await _taiKhoanRepo.GetByIdAsync(dto.Ma_TaiKhoan);
            if (taiKhoan == null) return BadRequest("Tài khoản liên kết không tồn tại. ");

            // Nếu tài khoản đã có nhân viên -> tranh chấp
            if (await _nhanVienRepo.ExistsByAccountIdAsync(dto.Ma_TaiKhoan))
                return BadRequest("Tài khoản này đã được gán cho nhân viên khác.");

            var nv = new NhanVien
            {
                Ma_TaiKhoan = dto.Ma_TaiKhoan,
                HoTen = dto.Ho_Ten,
                SoDienThoai = dto.So_Dien_Thoai,
                DiaChi = dto.Dia_Chi,
                //Hinh_Anh = dto.Hinh_Anh
            };

            var success = await _nhanVienRepo.AddAsync(nv);
            if (!success) return StatusCode(500, "Tạo nhân viên thất bại.");

            // Lấy lại để trả ra đầy đủ (kèm vai trò)
            var createdNv = await _nhanVienRepo.GetByIdAsync(dto.Ma_TaiKhoan);
            return Ok(new
            {
                message = "Thêm nhân viên thành công!",
                //data = new
                //{
                //    maNhanVien = createdNv!.Ma_NhanVien,
                //    hoTen = createdNv.HoTen,
                //    soDienThoai = createdNv.SoDienThoai,
                //    diaChi = createdNv.DiaChi,
                //    hinhAnh = createdNv.Hinh_Anh,
                //    TaiKhoan = new
                //    {
                //        maTaiKhoan = createdNv.TaiKhoan!.Ma_TaiKhoan,
                //        tenDangNhap = createdNv.TaiKhoan.Ten_DangNhap,
                //        email = createdNv.TaiKhoan.Email,
                //        vaiTro = createdNv.TaiKhoan.VaiTro?.Ten_VaiTro
                //    }
                //}
            });
        }

        // PUT: Cập nhật toàn bộ nhân viên
        [HttpPut("{maNhanVien}")]
        public async Task<IActionResult> UpdateFull(int maNhanVien, [FromBody] NhanVienDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var existingNv = await _nhanVienRepo.GetByIdAsync(maNhanVien);
            if (existingNv == null)
                return NotFound($"Không tìm thấy nhân viên với mã = {maNhanVien}");

            // Bắt buộc nhập tất cả các trường
            existingNv.HoTen = dto.Ho_Ten!;
            existingNv.SoDienThoai = dto.So_Dien_Thoai!;
            existingNv.DiaChi = dto.Dia_Chi!;
            existingNv.Hinh_Anh = dto.Hinh_Anh;
            // Không đổi mã tài khoản

            var success = await _nhanVienRepo.UpdateAsync(existingNv);
            if (!success) return StatusCode(500, "Cập nhật thất bại.");

            return Ok(new { message = "Cập nhật nhân viên thành công!"});
        }

        // PATCH: Cập nhật một phần nhân viên
        [HttpPatch("{maNhanVien}")]
        public async Task<IActionResult> UpdatePartial(int maNhanVien, [FromBody] NhanVienDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var existingNv = await _nhanVienRepo.GetByIdAsync(maNhanVien);
            if (existingNv == null)
                return NotFound($"Không tìm thấy nhân viên với mã = {maNhanVien}");

            // Cập nhật từng trường nếu người dùng gửi
            if (!string.IsNullOrEmpty(dto.Ho_Ten)) existingNv.HoTen = dto.Ho_Ten;
            if (!string.IsNullOrEmpty(dto.So_Dien_Thoai)) existingNv.SoDienThoai = dto.So_Dien_Thoai;
            if (!string.IsNullOrEmpty(dto.Dia_Chi)) existingNv.DiaChi = dto.Dia_Chi;
            if (!string.IsNullOrEmpty(dto.Hinh_Anh)) existingNv.Hinh_Anh = dto.Hinh_Anh;

            // Cập nhật tài khoản nếu người dùng gửi khác với hiện tại
            if (dto.Ma_TaiKhoan != 0 && dto.Ma_TaiKhoan != existingNv.Ma_TaiKhoan)
            {
                var taiKhoan = await _taiKhoanRepo.GetByIdAsync(dto.Ma_TaiKhoan);
                if (taiKhoan == null)
                    return BadRequest("Tài khoản liên kết không tồn tại.");
                if (await _nhanVienRepo.ExistsByAccountIdAsync(dto.Ma_TaiKhoan))
                    return BadRequest("Tài khoản này đã được gán cho nhân viên khác.");

                existingNv.Ma_TaiKhoan = dto.Ma_TaiKhoan;
            }

            var success = await _nhanVienRepo.UpdateAsync(existingNv);
            if (!success) return StatusCode(500, "Cập nhật thất bại.");

            // Lấy lại dữ liệu đầy đủ (có vai trò)
            var updatedNv = await _nhanVienRepo.GetByIdAsync(maNhanVien);
            return Ok(new
            {
                message = "Cập nhật nhân viên thành công!",
                data = new
                {
                    maNhanVien = updatedNv!.Ma_NhanVien,
                    hoTen = updatedNv.HoTen,
                    soDienThoai = updatedNv.SoDienThoai,
                    diaChi = updatedNv.DiaChi,
                    hinhAnh = updatedNv.Hinh_Anh,
                    taiKhoan = new
                    {
                        maTaiKhoan = updatedNv.TaiKhoan!.Ma_TaiKhoan,
                        tenDangNhap = updatedNv.TaiKhoan.Ten_DangNhap,
                        email = updatedNv.TaiKhoan.Email,
                        vaiTro = updatedNv.TaiKhoan.VaiTro?.Ten_VaiTro
                    }
                }
            });
        }

        // DELETE: Xóa nhân viên theo mã
        [HttpDelete("{maNhanVien}")]
        public async Task<IActionResult> Delete(int maNhanVien)
        {
            // Kiểm tra xem nhân viên có tồn tại không
            var existingNv = await _nhanVienRepo.GetByIdAsync(maNhanVien);
            if (existingNv == null)
                return NotFound($"Không tìm thấy nhân viên với mã = {maNhanVien}");

            // Thực hiện xóa
            var success = await _nhanVienRepo.DeleteAsync(maNhanVien);
            if (!success)
                return StatusCode(500, "Xóa nhân viên thất bại.");

            return Ok(new { message = "Xóa nhân viên thành công!" });
        }
    }
}
