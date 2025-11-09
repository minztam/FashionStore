using FashionStore.Data;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly FashionStoreContext _context;
        public NhanVienRepository(FashionStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.NhanViens
                 .Include(n => n.TaiKhoan)
                 .ThenInclude(tk => tk!.VaiTro)
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<bool> AddAsync(NhanVien nv)
        {
            await _context.NhanViens.AddAsync(nv);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<NhanVien?> GetByIdAsync(int id)
        {
            return await _context.NhanViens
                .Include(n => n.TaiKhoan)
                .ThenInclude(tk => tk!.VaiTro)
                .FirstOrDefaultAsync(n => n.Ma_NhanVien == id);
        }

        public async Task<bool> UpdateAsync(NhanVien nv)
        {
            var existing = await _context.NhanViens.FindAsync(nv.Ma_NhanVien);
            if (existing == null) return false;

            existing.HoTen = nv.HoTen;
            existing.SoDienThoai = nv.SoDienThoai;
            existing.DiaChi = nv.DiaChi;
            existing.Hinh_Anh = nv.Hinh_Anh;
            existing.Ma_TaiKhoan = nv.Ma_TaiKhoan;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return false;

            _context.NhanViens.Remove(nv!);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByAccountIdAsync(int maTaiKhoan)
        {
            return await _context.NhanViens.AnyAsync(n => n.Ma_TaiKhoan == maTaiKhoan);
        }
    }
}
