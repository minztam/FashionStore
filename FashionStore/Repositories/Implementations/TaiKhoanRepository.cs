using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly FashionStoreContext _context;
        public TaiKhoanRepository(FashionStoreContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ danh sách tài khoản
        public async Task<IEnumerable<TaiKhoan>> GetAllAsync()
        {
            return await _context.TaiKhoans
                                    .Include(t => t.VaiTro) // Muốn trả ra tên vai trò
                                    .ToListAsync();
        }

        //  Lấy tài khoản theo mã (maTaiKhoan)
        public async Task<TaiKhoan?> GetByIdAsync(int maTaiKhoan)
        {
            return (await _context.TaiKhoans
                                    .Include(t => t.VaiTro)
                                    .SingleOrDefaultAsync(t => t.Ma_TaiKhoan == maTaiKhoan));
        }

        // Thêm mới tài khoản (đăng ký hoặc tạo bởi admin)
        public async Task<bool> AddAsync(string tenDangNhap, string matKhau, string maVaiTro, string? email)
        {
            // Kiểm tra vai trò khách hàng
            bool isKhachHang = maVaiTro == "2222-2222-2222-2222";
            if (isKhachHang && string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email là bắt buộc đối với khách hàng!");
            }

            // Kiểm tra trùng tên đăng nhập
            if (await _context.TaiKhoans.AnyAsync(t => t.Ten_DangNhap == tenDangNhap))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại!");

            // Nếu có email, kiểm tra trùng email
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (await _context.TaiKhoans.AnyAsync(t => t.Email == email))
                    throw new InvalidOperationException("Email đã tồn tại!");
            }

            // Tạo token xác thực
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Tạo đối tượng TaiKhoan
            var taiKhoan = new TaiKhoan
            {
                Ten_DangNhap = tenDangNhap,
                Mat_Khau = matKhau,
                Ma_VaiTro = maVaiTro,
                Email = email ?? string.Empty,
                Da_XacThuc = false,
                Ma_XacThuc = token,
                Han_XacThuc = DateTime.UtcNow.AddDays(1)
            };

            _context.TaiKhoans.Add(taiKhoan);
            return await _context.SaveChangesAsync() > 0;
        }

        // Cập nhật toàn bộ thông tin tài khoản
        public async Task<bool> UpdateAsync(int maTaiKhoan, string? tenDangNhap, string? matKhau, string? maVaiTro, string? email)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            if (taiKhoan == null)
                return false;

            // Cập nhật tên đăng nhập nếu có truyền vào
            if (!string.IsNullOrWhiteSpace(tenDangNhap))
            {
                // Kiểm tra trùng tên đăng nhập
                if (await _context.TaiKhoans.AnyAsync(t => t.Ten_DangNhap == tenDangNhap && t.Ma_TaiKhoan != maTaiKhoan))
                    throw new InvalidOperationException("Tên đăng nhập đã tồn tại!");
                taiKhoan.Ten_DangNhap = tenDangNhap;
            }

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrWhiteSpace(matKhau))
                taiKhoan.Mat_Khau = matKhau;

            // Cập nhật vai trò nếu có
            if (!string.IsNullOrWhiteSpace(maVaiTro))
            {
                var vaiTro = await _context.VaiTros.FindAsync(maVaiTro);
                if (vaiTro == null)
                    throw new InvalidOperationException("Vai trò không tồn tại!");
                taiKhoan.Ma_VaiTro = maVaiTro;
                taiKhoan.VaiTro = vaiTro;
            }

            // Cập nhật email nếu có
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Kiểm tra trùng email
                if (await _context.TaiKhoans.AnyAsync(t => t.Email == email && t.Ma_TaiKhoan != maTaiKhoan))
                    throw new InvalidOperationException("Email đã tồn tại!");
                taiKhoan.Email = email;
            }

            _context.TaiKhoans.Update(taiKhoan);
            return await _context.SaveChangesAsync() > 0;
        }

        //Cập nhật một phần thông tin tài khoản (PATCH)
        public async Task<bool> UpdatePartialAsync(int maTaiKhoan, TaiKhoanDTO dto)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            if (taiKhoan == null)
                throw new InvalidOperationException("Tài khoản không tồn tại!");

            // Cập nhật tên đăng nhập nếu có
            if (!string.IsNullOrWhiteSpace(dto.Ten_DangNhap) && dto.Ten_DangNhap != taiKhoan.Ten_DangNhap)
            {
                if (await _context.TaiKhoans.AnyAsync(t => t.Ten_DangNhap == dto.Ten_DangNhap))
                    throw new InvalidOperationException("Tên đăng nhập đã tồn tại!");
                taiKhoan.Ten_DangNhap = dto.Ten_DangNhap;
            }

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrWhiteSpace(dto.Mat_Khau))
            {
                taiKhoan.Mat_Khau = dto.Mat_Khau;
            }

            // Cập nhật email nếu có
            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != taiKhoan.Email)
            {
                if (await _context.TaiKhoans.AnyAsync(t => t.Email == dto.Email))
                    throw new InvalidOperationException("Email đã tồn tại!");
                taiKhoan.Email = dto.Email;
            }

            // Cập nhật vai trò nếu có
            if (!string.IsNullOrWhiteSpace(dto.Ma_VaiTro) && dto.Ma_VaiTro != taiKhoan.Ma_VaiTro)
            {
                var vaiTro = await _context.VaiTros.FindAsync(dto.Ma_VaiTro);
                if (vaiTro == null)
                    throw new InvalidOperationException("Vai trò không tồn tại!");
                taiKhoan.Ma_VaiTro = dto.Ma_VaiTro;
                taiKhoan.VaiTro = vaiTro;
            }

            _context.TaiKhoans.Update(taiKhoan);
            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa tài khoản(và xóa luôn bản ghi KhachHang/NhanVien liên kết)
        public async Task<bool> DeleteAsync(int maTaiKhoan)
        {
            // Lấy tài khoản cùng các bảng liên quan
            var taiKhoan = await _context.TaiKhoans
                                  .Include(x => x.KhachHang)
                                  .Include(x => x.NhanVien)
                                  .FirstOrDefaultAsync(x => x.Ma_TaiKhoan == maTaiKhoan);

            if (taiKhoan == null)
                return false; // Không tìm thấy tài khoản

            // Xóa các bản ghi liên quan nếu có
            if (taiKhoan.KhachHang != null)
                _context.KhachHangs.Remove(taiKhoan.KhachHang);

            if (taiKhoan.NhanVien != null)
                _context.NhanViens.Remove(taiKhoan.NhanVien);

            // Xóa tài khoản chính
            _context.TaiKhoans.Remove(taiKhoan);

            // Lưu thay đổi
            return await _context.SaveChangesAsync() > 0;
        }

        //Đăng nhập (kiểm tra username + password)
        public async Task<TaiKhoan?> LoginAsync(string username, string password)
        {
            return await _context.TaiKhoans 
                .Include(t => t.VaiTro)
                .FirstOrDefaultAsync(t => t.Ten_DangNhap == username && t.Mat_Khau == password);
        }

        public async Task<TaiKhoan?> RegisterCustomerAsync(RegisterDTO dto)
        {
            // Kiểm tra trùng username
            if (await _context.TaiKhoans.AnyAsync(x => x.Ten_DangNhap == dto.Ten_DangNhap))
                return null;

            // Kiểm tra trùng email
            if (await _context.TaiKhoans.AnyAsync(x => x.Email == dto.Email))
                return null;

            if (string.IsNullOrWhiteSpace(dto.Mat_Khau))
                return null;

            // Hash mật khẩu
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Mat_Khau);

            // Tạo token xác thực
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Tạo tài khoản
            var taiKhoan = new TaiKhoan
            {
                Ten_DangNhap = dto.Ten_DangNhap,
                Email = dto.Email,
                Mat_Khau = dto.Mat_Khau,
                Ma_VaiTro = "2222-2222-2222-2222", // khách hàng
                Ma_XacThuc = token,
                Han_XacThuc = DateTime.UtcNow.AddDays(1)
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync(); // Lưu để sinh Ma_TaiKhoan

            // Tạo khách hàng (Ma_KhachHang sẽ tự tăng)
            var kh = new KhachHang
            {
                Ma_TaiKhoan = taiKhoan.Ma_TaiKhoan
            };

            _context.KhachHangs.Add(kh);
            await _context.SaveChangesAsync();

            return taiKhoan;
        }

        // Gán vai trò cho tài khoản (thay đổi role)
        public async Task<bool> AssignRoleAsync(int taiKhoanId, string roleId)
        {
            var tk = await _context.TaiKhoans.FindAsync(taiKhoanId);
            if (tk == null) return false;

            tk.Ma_VaiTro = roleId;
            return await _context.SaveChangesAsync() > 0;
        }

        // Kiểm tra tồn tại theo tên đăng nhập
        public async Task<bool> ExistsByUsernameAsync(string ten_DangNhap)
        {
            if(string.IsNullOrWhiteSpace(ten_DangNhap))
        throw new ArgumentException("Tên đăng nhập không được để trống!", nameof(ten_DangNhap));

            return await _context.TaiKhoans
                                 .AnyAsync(t => t.Ten_DangNhap == ten_DangNhap);
        }

        // Kiểm tra tồn tại theo email
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.TaiKhoans
                                 .AsNoTracking()   // tránh bị ChangeTracker làm sai lệch
                                 .AnyAsync(e => e.Email == email);
        }

        // Khóa / mở khóa tài khoản (đảo trạng thái)
        public async Task<bool> ToggleStatusAsync(int maTaiKhoan)
        {
            var tk = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            if (tk == null) return false;

            tk.Trang_Thai = !tk.Trang_Thai; // đảo trạng thái
            _context.TaiKhoans.Update(tk);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
