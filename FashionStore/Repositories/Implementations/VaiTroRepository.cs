using FashionStore.Data;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly FashionStoreContext _context;
        public VaiTroRepository(FashionStoreContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(VaiTro entity)
        {
            // Kiểm tra trùng ID
            if (await _context.VaiTros.AnyAsync(v => v.Ma_VaiTro == entity.Ma_VaiTro))
                throw new InvalidOperationException("Mã vai trò đã tồn tại!");

            // Kiểm tra trùng Tên Vai Trò
            if (await _context.VaiTros.AnyAsync(v => v.Ten_VaiTro == entity.Ten_VaiTro))
                throw new InvalidOperationException("Tên vai trò đã tồn tại!");

            _context.VaiTros.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string maVaiTro)
        {
            var vaiTro = await _context.VaiTros.FindAsync(maVaiTro);
            if (vaiTro == null) return false; // trả về false nếu không tìm thấy

            // Load navigation property nếu muốn xóa các TaiKhoan liên quan
            // await _context.Entry(vaiTro).Collection(v => v.TaiKhoans).LoadAsync(); 
            // nếu không muốn load, bỏ dòng trên

            _context.VaiTros.Remove(vaiTro);
            var result = await _context.SaveChangesAsync();
            return result > 0; // true nếu xóa thành công
        }

        public async Task<IEnumerable<VaiTro>> GetAllAsync()
        {
            return await _context.VaiTros.ToListAsync();
        }

        public async Task<VaiTro?> GetByIdAsync(string maVaiTro)
        {
            return await _context.VaiTros.FindAsync(maVaiTro);
        }

        public async Task<bool> UpdateAsync(string id, string tenVaiTroMoi)
        {
            // Tìm VaiTro theo ID
            var existingRole = await _context.VaiTros.FindAsync(id);
            if (existingRole == null)
                throw new InvalidOperationException("Vai trò không tồn tại!");

            // Cập nhật thông tin
            existingRole.Ten_VaiTro = tenVaiTroMoi;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
