using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;


namespace FashionStore.Repositories.Implementations
{
    public class DanhMucRepository(FashionStoreContext _context,ResponseMessageResult _response) : IDanhMucRepository
    {
        public async Task<ResponseMessageResult> GetAllAsync()
        {
            try
            {
                var rs = await _context.DanhMucs.Select(dm => new DanhMucDTO
                {
                    Ma_DanhMuc = dm.Ma_DanhMuc,
                    Ten_DanhMuc = dm.Ten_DanhMuc,
                    Ma_DanhMucCha = dm.Ma_DanhMucCha,
                    Ten_DanhMucCha = dm.DanhMucCha != null ? dm.DanhMucCha.Ten_DanhMuc : null,
                    Trang_Thai = dm.Trang_Thai,
                })
               .ToListAsync();
                if (rs.Count == 0) {
                    _response.SetCustom(true, null!, 200, null);
                }
                _response.SetSuccess("Lấy danh sách danh mục thành công", rs);
                return _response;
            }
            catch (Exception)
            {

                _response.SetCustom(false, "Có lỗi trong quá trình lấy danh sách danh mục", 500, null);
                return _response;
            }
        }

        public async Task<DanhMucDTO?> GetByIdAsync(string maDanhMuc)
        {
            return await _context.DanhMucs
                .Where(dm => dm.Ma_DanhMuc == maDanhMuc)
                .Select(dm => new DanhMucDTO
                {
                    Ma_DanhMuc = dm.Ma_DanhMuc,
                    Ten_DanhMuc = dm.Ten_DanhMuc,
                    Ma_DanhMucCha = dm.Ma_DanhMucCha,
                    Ten_DanhMucCha = dm.DanhMucCha != null ? dm.DanhMucCha.Ten_DanhMuc : null,
                    Trang_Thai = dm.Trang_Thai
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DanhMucDTO>> SearchAsync(string? keyword)
        {
            var query = _context.DanhMucs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(d => d.Ten_DanhMuc.Contains(keyword));
            }

            return await query
                .OrderBy(d => d.Ten_DanhMuc)
                .Take(20)
                .Select(d => new DanhMucDTO
                {
                    Ma_DanhMuc = d.Ma_DanhMuc,
                    Ten_DanhMuc = d.Ten_DanhMuc
                })
                .ToListAsync();
        }

        public async Task<ResponseMessageResult> GetTreeAsync()
        {
            try
            {
                var allCategories = await _context.DanhMucs
                    .AsNoTracking()
                    .Where(d => d.Trang_Thai)
                    .Select(d => new DanhMucDTO
                    {
                        Ma_DanhMuc = d.Ma_DanhMuc,
                        Ten_DanhMuc = d.Ten_DanhMuc,
                        Ma_DanhMucCha = d.Ma_DanhMucCha,
                        Ten_DanhMucCha = d.DanhMucCha != null ? d.DanhMucCha.Ten_DanhMuc : null,
                        Trang_Thai = d.Trang_Thai
                    })
                    .ToListAsync();

                var lookup = allCategories.ToLookup(x => x.Ma_DanhMucCha);

                //foreach (var cat in allCategories)
                //{
                //    cat.DanhMucCon = lookup[cat.Ma_DanhMuc].OrderBy(c => c.Ten_DanhMuc).ToList();
                //}

                var rootCategories = allCategories
                    .Where(x => string.IsNullOrEmpty(x.Ma_DanhMucCha))
                    .OrderBy(x => x.Ten_DanhMuc)
                    .ToList();

                return _response.SetSuccess("Lấy cây danh mục thành công!", rootCategories);
            }
            catch (Exception)
            {
                //_logger?.LogError(ex, "Lỗi khi lấy cây danh mục");
                return _response.SetFail("Không thể tải danh mục!", 500);
            }
        }

        public async Task<bool> AddAsync(ThemDanhMucDTO danhMuc)
        {
            if (danhMuc == null) return false;

            danhMuc.Ten_DanhMuc = danhMuc.Ten_DanhMuc?.Trim();
            if (string.IsNullOrWhiteSpace(danhMuc.Ten_DanhMuc))
                return false;

            string newCode = "";

            // ============================
            // 1) NẾU KHÔNG CÓ MÃ CHA → LÀ DANH MỤC CHA
            // ============================
            if (string.IsNullOrWhiteSpace(danhMuc.Ma_DanhMucCha))
            {
                var lastParent = await _context.DanhMucs
                    .Where(x => x.Ma_DanhMuc.Length == 5)        // chỉ lấy DM001, DM010...
                    .OrderByDescending(x => x.Ma_DanhMuc)
                    .Select(x => x.Ma_DanhMuc)
                    .FirstOrDefaultAsync();

                if (lastParent == null)
                {
                    newCode = "DM003";
                }
                else
                {
                    int number = int.Parse(lastParent.Substring(2));
                    number++;
                    newCode = "DM" + number.ToString("D3");
                }
            }
            else
            {
                // ============================
                // 2) NẾU CÓ MÃ CHA → LÀ DANH MỤC CON
                // tạo theo dạng: mã cha + chữ cái A,B,C...
                // ============================
                string parent = danhMuc.Ma_DanhMucCha;

                var lastChild = await _context.DanhMucs
                    .Where(x => x.Ma_DanhMuc.StartsWith(parent)
                                && x.Ma_DanhMuc.Length == parent.Length + 1)
                    .OrderByDescending(x => x.Ma_DanhMuc)
                    .Select(x => x.Ma_DanhMuc)
                    .FirstOrDefaultAsync();

                if (lastChild == null)
                {
                    newCode = parent + "A";
                }
                else
                {
                    char lastChar = lastChild.Last();
                    char nextChar = (char)(lastChar + 1);
                    newCode = parent + nextChar;
                }
            }

            // Map DTO → Entity
            var entity = new DanhMuc
            {
                Ma_DanhMuc = newCode,
                Ten_DanhMuc = danhMuc.Ten_DanhMuc,
                Ma_DanhMucCha = danhMuc.Ma_DanhMucCha,
                Trang_Thai = danhMuc.Trang_Thai
            };

            try
            {
                await _context.DanhMucs.AddAsync(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }




        public async Task<bool> UpdateAsync(string maDanhMuc, ThemDanhMucDTO dto)
        {
            if (dto == null) return false;

            var existing = await _context.DanhMucs
                .FirstOrDefaultAsync(x => x.Ma_DanhMuc == maDanhMuc);

            if (existing == null) return false;

            // Chuẩn hóa dữ liệu
            dto.Ten_DanhMuc = dto.Ten_DanhMuc?.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Ma_DanhMucCha))
                dto.Ma_DanhMucCha = dto.Ma_DanhMucCha.Trim().ToUpper();

            // ===== KIỂM TRA DANH MỤC CHA =====
            if (!string.IsNullOrWhiteSpace(dto.Ma_DanhMucCha))
            {
                // Không được đặt chính nó làm cha
                if (dto.Ma_DanhMucCha == maDanhMuc)
                    return false;

                var parentExists = await _context.DanhMucs
                    .AnyAsync(x => x.Ma_DanhMuc == dto.Ma_DanhMucCha);

                if (!parentExists)
                    return false;
            }

            // ===== CẬP NHẬT =====
            existing.Ten_DanhMuc = dto.Ten_DanhMuc;
            existing.Ma_DanhMucCha = dto.Ma_DanhMucCha;
            existing.Trang_Thai = dto.Trang_Thai;

            try
            {
                _context.DanhMucs.Update(existing);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            id = id.Trim().ToUpper();

            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return false;

            // Kiểm tra có danh mục con
            bool hasChild = await _context.DanhMucs.AnyAsync(x => x.Ma_DanhMucCha == id);
            if (hasChild)
                return false; // hoặc throw new InvalidOperationException("Không thể xóa vì có danh mục con.");

            // Kiểm tra có sản phẩm thuộc danh mục
            bool hasProduct = await _context.SanPhams.AnyAsync(x => x.Ma_DanhMuc == id);
            if (hasProduct)
                return false; // hoặc throw new InvalidOperationException("Không thể xóa vì danh mục đang chứa sản phẩm.");

            try
            {
                _context.DanhMucs.Remove(dm);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ToggleStatusAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            id = id.Trim().ToUpper();

            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return false;

            dm.Trang_Thai = !dm.Trang_Thai;

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

    }
}
