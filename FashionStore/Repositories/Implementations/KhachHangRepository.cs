using FashionStore.Data;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly FashionStoreContext _context;
        public KhachHangRepository(FashionStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _context.KhachHangs.Include(n => n.TaiKhoan)
                .ThenInclude(tk => tk!.VaiTro)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<KhachHang?> GetByIdAsync(int id)
        {
            return await _context.KhachHangs
                .Include(n => n.TaiKhoan)
                .ThenInclude(tk => tk!.VaiTro)
                .FirstOrDefaultAsync(n => n.Ma_KhachHang == id);
        }

        public async Task<bool> AddAsync(KhachHang kh)
        {
            await _context.KhachHangs.AddAsync(kh);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(KhachHang kh)
        {
            _context.KhachHangs.Update(kh);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null) return false;

            _context.KhachHangs.Remove(kh);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByAccountIdAsync(int id)
        {
            return await _context.KhachHangs.AnyAsync(k => k.Ma_KhachHang == id);
        }
    }
}
