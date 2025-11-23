using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;

        public SanPhamRepository(FashionStoreContext context, ResponseMessageResult response)
        {
            _context = context;
            _response = response;
        }

        public async Task<ResponseMessageResult> GetAllAsync()
        {
            try
            {
                var sanphams = await _context.SanPhams
                    .Include(x => x.HinhAnhSanPhams)
                    .Include(x => x.DanhMuc)
                    .Include(x => x.BienThes)
                    .AsNoTracking()
                    .ToListAsync();

                if (sanphams != null)
                {
                    // Map sang SanPhamDTO
                    var data = sanphams.Select(sp => new SanPhamDTO
                    {
                        Ma_SanPham = sp.Ma_SanPham,
                        Ten_SanPham = sp.Ten_SanPham,
                        Ma_DanhMuc = sp.Ma_DanhMuc,
                        Mo_Ta = sp.Mo_Ta,
                        Trang_Thai = sp.Trang_Thai,
                        HinhAnhs = sp.HinhAnhSanPhams?.Select(h => new HinhAnhDTO
                        {
                            DuongDan = h.DuongDan ?? string.Empty
                        }).ToList(),
                        BienThes = sp.BienThes?.Select(bt => new SanPhamBienTheDTO
                        {
                            Mau_Sac = bt.Mau_Sac,
                            Kich_Thuoc = bt.Kich_Thuoc,
                            So_Luong = bt.So_Luong,
                            Gia_BienThe = bt.Gia_BienThe,
                            Gia_Giam = bt.Gia_Giam,
                            PhanTramGiam = bt.PhanTramGiam
                        }).ToList()
                    }).ToList();

                    _response.SetSuccess("Lấy danh sách sản phẩm thành công", data);
                    return _response;
                }
                _response.SetCustom(true, "Không có sản phẩm nào", 200, new List<SanPhamDTO>());
                return _response;
            }
            catch (Exception)
            {
                _response.SetCustom(false, "Có lỗi trong quá trình lấy danh sách sản phẩm", 500, null);
                return _response;
            }
        }

        public async Task<ResponseMessageResult> GetByIdAsync(string id)
        {
            try
            {
                var sp = await _context.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .Include(sp => sp.HinhAnhSanPhams)
                    .Include(sp => sp.BienThes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(sp => sp.Ma_SanPham == id);

                if (sp != null)
                {
                    var data = new SanPhamDTO
                    {
                        Ma_SanPham = sp.Ma_SanPham,
                        Ten_SanPham = sp.Ten_SanPham,
                        Ma_DanhMuc = sp.Ma_DanhMuc,
                        Mo_Ta = sp.Mo_Ta,
                        Trang_Thai = sp.Trang_Thai,
                        HinhAnhs = sp.HinhAnhSanPhams?.Select(h => new HinhAnhDTO
                        {
                            DuongDan = h.DuongDan ?? string.Empty
                        }).ToList(),
                        BienThes = sp.BienThes?.Select(bt => new SanPhamBienTheDTO
                        {
                            Mau_Sac = bt.Mau_Sac,
                            Kich_Thuoc = bt.Kich_Thuoc,
                            So_Luong = bt.So_Luong,
                            Gia_BienThe = bt.Gia_BienThe,
                            Gia_Giam = bt.Gia_Giam,
                            PhanTramGiam = bt.PhanTramGiam
                        }).ToList()
                    };

                    _response.SetSuccess("Lấy sản phẩm thành công", data);
                    return _response;
                }
                _response.SetFail("Sản phẩm không tồn tại", 404);
                return _response;
            }
            catch (Exception )
            {
                _response.SetCustom(false, "Có lỗi trong quá trình lấy sản phẩm", 500, null);
                return _response;
            }
        }

        public async Task<ResponseMessageResult> CreateAsync(SanPham sp, List<HinhAnhDTO>? hinhAnhs, List<SanPhamBienTheDTO>? bienThes = null)
        {
            if (sp == null)
                return _response.SetFail("Sản phẩm không được null", 400);

            try
            {
                // 1. Sinh mã sản phẩm tự động
                sp.Ma_SanPham = await GenerateMaSanPhamAsync();
                sp.Ten_SanPham = sp.Ten_SanPham.Trim();

                // 2. Kiểm tra danh mục hợp lệ
                bool danhMucExists = await _context.DanhMucs
                    .AnyAsync(x => x.Ma_DanhMuc == sp.Ma_DanhMuc);

                if (!danhMucExists)
                    return _response.SetFail($"Danh mục '{sp.Ma_DanhMuc}' không tồn tại.", 400);

                // 3. Bắt đầu transaction
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

                    // Thêm biến thể (nếu có)
                    if (bienThes != null && bienThes.Any())
                    {
                        var listBienThe = bienThes
                            .Where(bt => !string.IsNullOrWhiteSpace(bt.Mau_Sac) && !string.IsNullOrWhiteSpace(bt.Kich_Thuoc))
                            .Select(bt => new SanPhamBienThe
                            {
                                Ma_SanPham = sp.Ma_SanPham,
                                Mau_Sac = bt.Mau_Sac.Trim(),
                                Kich_Thuoc = bt.Kich_Thuoc.Trim(),
                                So_Luong = bt.So_Luong,
                                Gia_BienThe = bt.Gia_BienThe,
                                Gia_Giam = bt.Gia_Giam,
                                PhanTramGiam = bt.PhanTramGiam
                            });

                        await _context.SanPhamBienThes.AddRangeAsync(listBienThe);
                        await _context.SaveChangesAsync();
                    }

                    // Commit transaction
                    await transaction.CommitAsync();
                    return _response.SetSuccess("Thêm sản phẩm thành công", sp);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"❌ Lỗi khi thêm sản phẩm: {ex.Message}");
                    return _response.SetFail("Có lỗi khi thêm sản phẩm", 500);
                }
            }
            catch (Exception )
            {
                return _response.SetFail("Có lỗi không mong muốn", 500);
            }
        }

        public async Task<ResponseMessageResult> UpdateAsync(string maSanPham, SanPhamDTO dto)
        {
            try
            {
                var existing = await _context.SanPhams
                    .Include(sp => sp.HinhAnhSanPhams)
                    .Include(sp => sp.BienThes)
                    .FirstOrDefaultAsync(sp => sp.Ma_SanPham == maSanPham);

                if (existing == null)
                    return _response.SetFail("Sản phẩm không tồn tại", 404);

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Cập nhật sản phẩm
                    if (!string.IsNullOrWhiteSpace(dto.Ten_SanPham))
                        existing.Ten_SanPham = dto.Ten_SanPham.Trim();
                    if (!string.IsNullOrWhiteSpace(dto.Mo_Ta))
                        existing.Mo_Ta = dto.Mo_Ta.Trim();
                    existing.Trang_Thai = dto.Trang_Thai;
                    existing.Ma_DanhMuc = dto.Ma_DanhMuc;

                    // Cập nhật biến thể
                    if (dto.BienThes != null)
                    {
                        _context.SanPhamBienThes.RemoveRange(existing.BienThes);

                        var newBienThes = dto.BienThes.Select(bt => new SanPhamBienThe
                        {
                            Ma_SanPham = maSanPham,
                            Mau_Sac = bt.Mau_Sac.Trim(),
                            Kich_Thuoc = bt.Kich_Thuoc.Trim(),
                            So_Luong = bt.So_Luong,
                            Gia_BienThe = bt.Gia_BienThe,
                            Gia_Giam = bt.Gia_Giam,
                            PhanTramGiam = bt.PhanTramGiam
                        }).ToList();

                        await _context.SanPhamBienThes.AddRangeAsync(newBienThes);
                    }

                    // Cập nhật hình ảnh
                    if (dto.HinhAnhs != null && dto.HinhAnhs.Any())
                    {
                        _context.HinhAnhSanPhams.RemoveRange(existing.HinhAnhSanPhams);

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
                    await transaction.CommitAsync();
                    return _response.SetSuccess("Cập nhật sản phẩm thành công");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"❌ Lỗi khi cập nhật sản phẩm: {ex.Message}");
                    return _response.SetFail("Có lỗi khi cập nhật sản phẩm", 500);
                }
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi không mong muốn", 500);
            }
        }

        public async Task<ResponseMessageResult> DeleteAsync(string id)
        {
            try
            {
                var sp = await _context.SanPhams
                    .Include(s => s.HinhAnhSanPhams)
                    .Include(s => s.BienThes)
                    .FirstOrDefaultAsync(s => s.Ma_SanPham == id);

                if (sp == null)
                    return _response.SetFail("Sản phẩm không tồn tại", 404);

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.SanPhamBienThes.RemoveRange(sp.BienThes);
                    _context.HinhAnhSanPhams.RemoveRange(sp.HinhAnhSanPhams);
                    _context.SanPhams.Remove(sp);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _response.SetSuccess("Xóa sản phẩm thành công");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"❌ Lỗi khi xóa sản phẩm: {ex.Message}");
                    return _response.SetFail("Có lỗi khi xóa sản phẩm", 500);
                }
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi không mong muốn", 500);
            }
        }

        public async Task<ResponseMessageResult> AddImageAsync(HinhAnhSanPham img)
        {
            if (img == null || string.IsNullOrWhiteSpace(img.Ma_SanPham))
                return _response.SetFail("Thông tin hình ảnh không hợp lệ", 400);

            try
            {
                var spExists = await _context.SanPhams.AnyAsync(s => s.Ma_SanPham == img.Ma_SanPham);
                if (!spExists)
                    return _response.SetFail($"Sản phẩm {img.Ma_SanPham} không tồn tại.", 404);

                await _context.HinhAnhSanPhams.AddAsync(img);
                await _context.SaveChangesAsync();
                return _response.SetSuccess("Thêm hình ảnh thành công");
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi khi thêm hình ảnh", 500);
            }
        }

        public async Task<ResponseMessageResult> DeleteImageAsync(int id)
        {
            try
            {
                var img = await _context.HinhAnhSanPhams.FindAsync(id);
                if (img == null)
                    return _response.SetFail("Hình ảnh không tồn tại", 404);

                _context.HinhAnhSanPhams.Remove(img);
                await _context.SaveChangesAsync();
                return _response.SetSuccess("Xóa hình ảnh thành công");
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi khi xóa hình ảnh", 500);
            }
        }

        public async Task<ResponseMessageResult> PatchAsync(string id, SanPhamDTO dto)
        {
            try
            {
                var sp = await _context.SanPhams
                    .Include(s => s.HinhAnhSanPhams)
                    .Include(s => s.BienThes)
                    .FirstOrDefaultAsync(s => s.Ma_SanPham == id);

                if (sp == null)
                    return _response.SetFail("Sản phẩm không tồn tại", 404);

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    if (!string.IsNullOrWhiteSpace(dto.Ten_SanPham))
                        sp.Ten_SanPham = dto.Ten_SanPham.Trim();
                    if (!string.IsNullOrWhiteSpace(dto.Mo_Ta))
                        sp.Mo_Ta = dto.Mo_Ta.Trim();
                    sp.Trang_Thai = dto.Trang_Thai;
                    sp.Ma_DanhMuc = dto.Ma_DanhMuc;

                    // Xử lý biến thể
                    if (dto.BienThes != null)
                    {
                        _context.SanPhamBienThes.RemoveRange(sp.BienThes);

                        var newBienThes = dto.BienThes.Select(bt => new SanPhamBienThe
                        {
                            Ma_SanPham = id,
                            Mau_Sac = bt.Mau_Sac.Trim(),
                            Kich_Thuoc = bt.Kich_Thuoc.Trim(),
                            So_Luong = bt.So_Luong,
                            Gia_BienThe = bt.Gia_BienThe,
                            Gia_Giam = bt.Gia_Giam,
                            PhanTramGiam = bt.PhanTramGiam
                        });

                        await _context.SanPhamBienThes.AddRangeAsync(newBienThes);
                    }

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
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _response.SetSuccess("Cập nhật sản phẩm thành công");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"❌ Lỗi khi patch sản phẩm: {ex.Message}");
                    return _response.SetFail("Có lỗi khi cập nhật sản phẩm", 500);
                }
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi không mong muốn", 500);
            }
        }

        public async Task<ResponseMessageResult> ToggleStatusAsync(string id)
        {
            try
            {
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.Ma_SanPham == id);
                if (sp == null)
                    return _response.SetFail("Sản phẩm không tồn tại", 404);

                sp.Trang_Thai = !sp.Trang_Thai;
                _context.SanPhams.Update(sp);
                await _context.SaveChangesAsync();
                return _response.SetSuccess("Cập nhật trạng thái thành công");
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi khi cập nhật trạng thái", 500);
            }
        }

        public async Task<ResponseMessageResult> CreateBienTheAsync(string maSanPham, SanPhamBienTheDTO dto)
        {
            try
            {
                var spExists = await _context.SanPhams.AnyAsync(s => s.Ma_SanPham == maSanPham);
                if (!spExists)
                    return _response.SetFail($"Sản phẩm {maSanPham} không tồn tại.", 404);

                var bienThe = new SanPhamBienThe
                {
                    Ma_SanPham = maSanPham,
                    Mau_Sac = dto.Mau_Sac.Trim(),
                    Kich_Thuoc = dto.Kich_Thuoc.Trim(),
                    So_Luong = dto.So_Luong,
                    Gia_BienThe = dto.Gia_BienThe,
                    Gia_Giam = dto.Gia_Giam,
                    PhanTramGiam = dto.PhanTramGiam
                };

                await _context.SanPhamBienThes.AddAsync(bienThe);
                await _context.SaveChangesAsync();
                return _response.SetSuccess("Thêm biến thể thành công");
            }
            catch (Exception)
            {
                return _response.SetFail("Có lỗi khi thêm biến thể", 500);
            }
        }
        public async Task<ResponseMessageResult> TimKiemSanPhamAsync(SanPhamFilterDTO dto)
        {
            try
            {
                var query = _context.SanPhams
                    .Include(p => p.DanhMuc)
                    .Include(p => p.BienThes)
                    .Include(p => p.HinhAnhSanPhams)
                    .AsQueryable();

                //  Tìm theo tên
                if (!string.IsNullOrEmpty(dto.TuKhoa))
                    query = query.Where(p => p.Ten_SanPham.Contains(dto.TuKhoa));

                //  Lọc danh mục
                if (!string.IsNullOrEmpty(dto.MaDanhMuc))
                    query = query.Where(p => EF.Functions.Like(p.Ma_DanhMuc!, $"%{dto.MaDanhMuc}%"));

                //  Lọc màu sắc
                if (!string.IsNullOrEmpty(dto.MauSac))
                    query = query.Where(p => p.BienThes.Any(b => EF.Functions.Like(b.Mau_Sac!, $"%{dto.MauSac}%")));

                //  Lọc kích thước
                if (!string.IsNullOrEmpty(dto.KichThuoc))
                    query = query.Where(p => p.BienThes.Any(b => b.Kich_Thuoc == dto.KichThuoc));

                //  Lọc theo khoảng giá
                if (dto.GiaTu.HasValue)
                    query = query.Where(p => p.BienThes.Any(b => b.Gia_BienThe >= dto.GiaTu.Value));

                if (dto.GiaDen.HasValue)
                    query = query.Where(p => p.BienThes.Any(b => b.Gia_BienThe <= dto.GiaDen.Value));

                //  Còn hàng
                if (dto.ConHang.HasValue)
                {
                    if (dto.ConHang.Value)
                        query = query.Where(p => p.BienThes.Any(b => b.So_Luong > 0));
                    else
                        query = query.Where(p => p.BienThes.All(b => b.So_Luong == 0));
                }

                //  Sắp xếp  
                query = dto.SortBy?.ToLower() switch
                {
                    "gia_tang" => query.OrderBy(p => p.BienThes.Min(b => b.Gia_BienThe)),
                    "gia_giam" => query.OrderByDescending(p => p.BienThes.Max(b => b.Gia_BienThe)),
                    "moi_nhat" => query.OrderByDescending(p => p.Ma_SanPham), // có thể thay bằng ngày tạo nếu có
                    _ => query.OrderBy(p => p.Ten_SanPham)
                };

                //  Phân trang
                var total = await query.CountAsync();
                var data = await query
                    .Skip((dto.Page - 1) * dto.PageSize)
                    .Take(dto.PageSize)
                    .Select(p => new
                    {
                        p.Ma_SanPham,
                        p.Ten_SanPham,
                        p.Mo_Ta,
                        p.Trang_Thai,
                        DanhMuc = p.DanhMuc!.Ten_DanhMuc,
                        HinhAnh = p.HinhAnhSanPhams.Select(h => h.DuongDan).ToList(),

                        // giá thấp nhất trong biến thể
                        GiaThapNhat = p.BienThes.Any() ? p.BienThes.Min(b => b.Gia_BienThe) : 0,
                        BienThes = p.BienThes.Select(b => new {
                            b.Id,
                            b.Mau_Sac,
                            b.Kich_Thuoc,
                            b.So_Luong,
                            b.Gia_BienThe,
                            b.Gia_Giam,
                            b.PhanTramGiam
                        })
                    })
                    .ToListAsync();

                return _response.SetSuccess("Danh sách sản phẩm", new
                {
                    Total = total,
                    Page = dto.Page,
                    PageSize = dto.PageSize,
                    Items = data
                });
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        private async Task<string> GenerateMaSanPhamAsync()
        {
            var lastProduct = await _context.SanPhams
                .OrderByDescending(sp => sp.Ma_SanPham)
                .FirstOrDefaultAsync();

            if (lastProduct == null)
                return "SP001";

            string lastCode = lastProduct.Ma_SanPham.Replace("SP", "");
            if (int.TryParse(lastCode, out int num))
                return $"SP{(num + 1):D3}";

            return $"SP{Guid.NewGuid().ToString("N")[..3].ToUpper()}";
        }
    }
}
