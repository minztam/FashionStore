using Azure;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;

        public GioHangRepository(FashionStoreContext context, ResponseMessageResult response)
        {
            _context = context;
            _response = response;
        }

        public async Task<ResponseMessageResult> GetCartAsync(int maKhachHang)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                        .ThenInclude(sp => sp!.BienThes)
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                        .ThenInclude(sp => sp!.HinhAnhSanPhams)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetSuccess("Giỏ hàng trống!", new GioHangDTO
                {
                    Ma_GioHang = 0,
                    Ma_KhachHang = maKhachHang,
                    SanPhams = new List<ChiTietGioHangDTO>()
                });

            // Chuyển chi tiết giỏ hàng sang DTO
            var chiTietDtos = gioHang.ChiTietGioHangs.Select(ct =>
            {
                var sp = ct.SanPham!;
                var bienThe = sp.BienThes.FirstOrDefault(); // Lấy biến thể đầu tiên
                decimal gia = bienThe?.Gia_BienThe ?? 0;
                decimal giaGiam = bienThe?.Gia_Giam ?? 0;

                return new ChiTietGioHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = sp.Ten_SanPham,
                    Hinh_Anh = sp.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                    Gia = gia,
                    Gia_Giam = giaGiam,
                    So_Luong = ct.So_Luong
                };
            }).ToList();

            // Tạo DTO giỏ hàng
            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = chiTietDtos
            };

            // TongTien sẽ tự tính từ property read-only của GioHangDTO
            return _response.SetSuccess("Lấy giỏ hàng thành công!", gioHangDto);
        }

        public async Task<ResponseMessageResult> AddToCartAsync(int maKhachHang, string maSanPham, int soLuong)
        {
            if (soLuong <= 0)
                return _response.SetFail("Số lượng phải lớn hơn 0!");

            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
            {
                gioHang = new GioHang { Ma_KhachHang = maKhachHang };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }

            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham == null)
                return _response.SetFail("Sản phẩm không tồn tại", 404);

            var existing = gioHang.ChiTietGioHangs.FirstOrDefault(x => x.Ma_SanPham == maSanPham);
            if (existing != null)
                existing.So_Luong += soLuong;
            else
                gioHang.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    Ma_GioHang = gioHang.Ma_GioHang,
                    Ma_SanPham = maSanPham,
                    So_Luong = soLuong
                });

            await _context.SaveChangesAsync();

            // --- Reload chi tiết giỏ hàng kèm sản phẩm ---
            gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                        .ThenInclude(sp => sp!.BienThes)
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                        .ThenInclude(sp => sp!.HinhAnhSanPhams)
                .FirstOrDefaultAsync(g => g.Ma_GioHang == gioHang.Ma_GioHang);

            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang!.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs.Select(ct =>
                {
                    var bienThe = ct.SanPham?.BienThes.FirstOrDefault(); // lấy biến thể đầu tiên
                    decimal gia = bienThe?.Gia_BienThe ?? 0;   // giá gốc
                    decimal giaGiam = bienThe?.Gia_Giam ?? 0;  // giá giảm
                    return new ChiTietGioHangDTO
                    {
                        Ma_SanPham = ct.Ma_SanPham,
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                        Hinh_Anh = ct.SanPham?.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                        Gia = gia,
                        Gia_Giam = giaGiam,
                        So_Luong = ct.So_Luong
                    };
                }).ToList()
            };
            return _response.SetSuccess("Thêm vào giỏ hàng thành công", gioHangDto);
        }

        public async Task<ResponseMessageResult> UpdateCartAsync(int maKhachHang, string maSanPham, int soLuong)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp!.BienThes)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetFail("Giỏ hàng không tồn tại", 404);

            var item = gioHang.ChiTietGioHangs.FirstOrDefault(x => x.Ma_SanPham == maSanPham);
            if (item == null)
                return _response.SetFail("Sản phẩm không có trong giỏ hàng", 404);

            if (soLuong <= 0)
                _context.ChiTietGioHangs.Remove(item);
            else
                item.So_Luong = soLuong;

            await _context.SaveChangesAsync();

            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs
                    .Where(ct => ct.So_Luong > 0)
                    .Select(ct =>
                    {
                        var bienThe = ct.SanPham?.BienThes.FirstOrDefault();
                        return new ChiTietGioHangDTO
                        {
                            Ma_SanPham = ct.Ma_SanPham,
                            Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                            Hinh_Anh = ct.SanPham?.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                            Gia = bienThe?.Gia_BienThe ?? 0,
                            Gia_Giam = bienThe?.Gia_Giam ?? 0,
                            So_Luong = ct.So_Luong
                        };
                    }).ToList()
            };

            return _response.SetSuccess("Cập nhật giỏ hàng thành công", gioHangDto);
        }

        public async Task<ResponseMessageResult> RemoveFromCartAsync(int maKhachHang, string maSanPham)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetFail("Giỏ hàng không tồn tại", 404);

            var item = gioHang.ChiTietGioHangs.FirstOrDefault(x => x.Ma_SanPham == maSanPham);
            if (item == null)
                return _response.SetFail("Sản phẩm không có trong giỏ hàng", 404);

            _context.ChiTietGioHangs.Remove(item);
            bool ok = await _context.SaveChangesAsync() > 0;
            return ok ? _response.SetSuccess("Xóa sản phẩm khỏi giỏ hàng thành công")
                      : _response.SetFail("Không thể xóa sản phẩm khỏi giỏ hàng");
        }
    }
}
