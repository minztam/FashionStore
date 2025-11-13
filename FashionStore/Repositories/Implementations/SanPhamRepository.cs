using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly FashionStoreContext _context;
        public SanPhamRepository(FashionStoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams
                .Include(x => x.HinhAnhSanPhams)
                .Include(x => x.DanhMuc)
                .ToListAsync();
        }
        public async Task<SanPham?> GetByIdAsync(string id)
        {
            return await _context.SanPhams
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.HinhAnhSanPhams)
                .FirstOrDefaultAsync(sp => sp.Ma_SanPham == id);
        }
        public async Task<bool> CreateAsync(SanPham sp, List<HinhAnhDTO>? hinhAnhs)
        {
            if (sp == null)
                throw new ArgumentNullException(nameof(sp));

            // 1. Sinh mã sản phẩm tự động
            sp.Ma_SanPham = await GenerateMaSanPhamAsync();
            sp.Ten_SanPham = sp.Ten_SanPham.Trim();

            // 2. Kiểm tra danh mục hợp lệ
            bool danhMucExists = await _context.DanhMuc
                .AnyAsync(x => x.Ma_DanhMuc == sp.Ma_DanhMuc);

            if (!danhMucExists)
                throw new InvalidOperationException($"Danh mục '{sp.Ma_DanhMuc}' không tồn tại.");

            // 3. Bắt đầu transaction để đảm bảo toàn vẹn dữ liệu
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Thêm sản phẩm
                await _context.SanPhams.AddAsync(sp);
                await _context.SaveChangesAsync();

                // Thêm hình ảnh (nếu có)
                if (hinhAnhs != null && hinhAnhs.Any())
                {
                    var listHinhAnh = hinhAnhs
                        .Where(h => !string.IsNullOrWhiteSpace(h.DuongDan))
                        .Select(h => new HinhAnhSanPham
                        {
                            Ma_SanPham = sp.Ma_SanPham,
                            DuongDan = h.DuongDan.Trim()
                        });

                    await _context.HinhAnhSanPhams.AddRangeAsync(listHinhAnh);
                    await _context.SaveChangesAsync();
                }

                // Commit transaction
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Lỗi khi thêm sản phẩm: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateAsync(string maSanPham, SanPhamDTO dto)
        {
            var existing = await _context.SanPhams
                .Include(sp => sp.HinhAnhSanPhams)
                .FirstOrDefaultAsync(sp => sp.Ma_SanPham == maSanPham);

            if (existing == null)
                return false;

            // Cập nhật các trường cơ bản
            existing.Ten_SanPham = dto.Ten_SanPham.Trim();
            existing.Mo_Ta = dto.Mo_Ta?.Trim();
            existing.Gia = dto.Gia;
            existing.Gia_Giam = dto.Gia_Giam;
            existing.So_Luong = dto.So_Luong;
            existing.Mau_Sac = dto.Mau_Sac;
            existing.Kich_Thuoc = dto.Kich_Thuoc;
            existing.Trang_Thai = dto.Trang_Thai;
            existing.Ma_DanhMuc = dto.Ma_DanhMuc;

            // Cập nhật hình ảnh (nếu có)
            if (dto.HinhAnhs != null && dto.HinhAnhs.Any())
            {
                // Xóa hình cũ
                _context.HinhAnhSanPhams.RemoveRange(existing.HinhAnhSanPhams);

                // Thêm hình mới
                var newImages = dto.HinhAnhs
                    .Where(h => !string.IsNullOrWhiteSpace(h.DuongDan))
                    .Select(h => new HinhAnhSanPham
                    {
                        Ma_SanPham = maSanPham,
                        DuongDan = h.DuongDan.Trim()
                    }).ToList();

                await _context.HinhAnhSanPhams.AddRangeAsync(newImages);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var sp = await _context.SanPhams
        .Include(s => s.HinhAnhSanPhams)
        .FirstOrDefaultAsync(s => s.Ma_SanPham == id);

            if (sp == null) return false;

            _context.HinhAnhSanPhams.RemoveRange(sp.HinhAnhSanPhams);
            _context.SanPhams.Remove(sp);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddImageAsync(HinhAnhSanPham img)
        {
            if (img == null || string.IsNullOrWhiteSpace(img.Ma_SanPham))
                throw new ArgumentNullException(nameof(img));

            var spExists = await _context.SanPhams.AnyAsync(s => s.Ma_SanPham == img.Ma_SanPham);
            if (!spExists)
                throw new InvalidOperationException($"Sản phẩm {img.Ma_SanPham} không tồn tại.");

            await _context.HinhAnhSanPhams.AddAsync(img);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteImageAsync(int id)
        {
            var img = await _context.HinhAnhSanPhams.FindAsync(id);
            if (img == null) return false;

            _context.HinhAnhSanPhams.Remove(img);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> PatchAsync(string id, SanPhamDTO dto)
        {
            var sp = await _context.SanPhams
                .Include(s => s.HinhAnhSanPhams)
                .FirstOrDefaultAsync(s => s.Ma_SanPham == id);

            if (sp == null) return false;

            // Cập nhật chuỗi nếu có dữ liệu
            if (!string.IsNullOrWhiteSpace(dto.Ten_SanPham))
                sp.Ten_SanPham = dto.Ten_SanPham.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Mo_Ta))
                sp.Mo_Ta = dto.Mo_Ta.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Mau_Sac))
                sp.Mau_Sac = dto.Mau_Sac.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Kich_Thuoc))
                sp.Kich_Thuoc = dto.Kich_Thuoc.Trim();

            // Cập nhật các trường giá trị số
            sp.Gia = dto.Gia;           // Giá luôn được gửi -> gán trực tiếp
            sp.So_Luong = dto.So_Luong; // Số lượng luôn được gửi -> gán trực tiếp
            sp.Trang_Thai = dto.Trang_Thai; // Trạng thái luôn được gửi -> gán trực tiếp

            if (dto.Gia_Giam.HasValue)
                sp.Gia_Giam = dto.Gia_Giam.Value;

            // Xử lý hình ảnh
            if (dto.HinhAnhs != null && dto.HinhAnhs.Any())
            {
                _context.HinhAnhSanPhams.RemoveRange(sp.HinhAnhSanPhams);

                var newImages = dto.HinhAnhs
                    .Where(h => !string.IsNullOrWhiteSpace(h.DuongDan))
                    .Select(h => new HinhAnhSanPham
                    {
                        Ma_SanPham = sp.Ma_SanPham,
                        DuongDan = h.DuongDan.Trim()
                    });

                await _context.HinhAnhSanPhams.AddRangeAsync(newImages);
            }

            _context.SanPhams.Update(sp);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ToggleStatusAsync(string id)
        {
            var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.Ma_SanPham == id);
            if (sp == null) return false;

            sp.Trang_Thai = !sp.Trang_Thai;
            _context.SanPhams.Update(sp);

            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> GenerateMaSanPhamAsync()
        {
            var lastProduct = await _context.SanPhams
                .OrderByDescending(sp => sp.Ma_SanPham)
                .FirstOrDefaultAsync();

            if (lastProduct == null)
                return "SP0001";

            string lastCode = lastProduct.Ma_SanPham.Replace("SP", "");
            if (int.TryParse(lastCode, out int num))
                return $"SP{(num + 1).ToString("D4")}";

            return $"SP{Guid.NewGuid().ToString("N")[..4].ToUpper()}";
        }
    }
}
