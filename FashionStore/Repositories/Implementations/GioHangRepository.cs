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

        public async Task<ResponseMessageResult> GetCartAsync(int maTaiKhoan)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maTaiKhoan);

            if (gioHang == null)
            {
                return _response.SetSuccess("Giỏ hàng trống!", new { Items = new List<object>() });
            }

            var result = gioHang.ChiTietGioHangs.Select(ct => new
            {
                ct.Ma_SanPham,
                ct.SanPham!.Ten_SanPham,
                ct.SanPham.Gia,
                ct.So_Luong,
                ThanhTien = ct.So_Luong * (ct.SanPham.Gia - (ct.SanPham.Gia_Giam ?? 0))
            });

            return _response.SetSuccess("Lấy giỏ hàng thành công!", result);
        }
        public async Task<ResponseMessageResult> AddToCartAsync(int maKhachHang, string maSanPham, int soLuong)
        {
            if (soLuong <= 0)
                return _response.SetFail("Số lượng phải lớn hơn 0!");

            // Lấy giỏ hàng kèm chi tiết
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            // Nếu chưa có giỏ, tạo mới
            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    Ma_KhachHang = maKhachHang
                };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync(); // Lưu để sinh Ma_GioHang
            }

            // Kiểm tra sản phẩm
            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham == null)
                return _response.SetFail("Sản phẩm không tồn tại", 404);

            // Thêm hoặc cập nhật số lượng
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

            // Chuyển sang DTO
            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs.Select(ct => new ChiTietGioHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                    Hinh_Anh = ct.SanPham?.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                    Gia = ct.SanPham?.Gia ?? 0,
                    Gia_Giam = ct.SanPham?.Gia_Giam ?? 0,
                    So_Luong = ct.So_Luong
                }).ToList()
            };

            return _response.SetSuccess("Thêm vào giỏ hàng thành công", gioHangDto);
        }
        public async Task<ResponseMessageResult> UpdateCartAsync(int maKhachHang, string maSanPham, int soLuong)
        {
            // Lấy giỏ hàng kèm chi tiết
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetFail("Giỏ hàng không tồn tại", 404);

            var item = gioHang.ChiTietGioHangs.FirstOrDefault(x => x.Ma_SanPham == maSanPham);
            if (item == null)
                return _response.SetFail("Sản phẩm không có trong giỏ hàng", 404);

            if (soLuong <= 0)
            {
                // Xóa sản phẩm khỏi giỏ
                _context.ChiTietGioHangs.Remove(item);
            }
            else
            {
                // Cập nhật số lượng
                item.So_Luong = soLuong;
            }

            await _context.SaveChangesAsync();

            // Trả về DTO giỏ hàng
            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs
                    .Where(ct => ct.So_Luong > 0)
                    .Select(ct => new ChiTietGioHangDTO
                    {
                        Ma_SanPham = ct.Ma_SanPham,
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                        Hinh_Anh = ct.SanPham?.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                        Gia = ct.SanPham?.Gia ?? 0,
                        Gia_Giam = ct.SanPham?.Gia_Giam ?? 0,
                        So_Luong = ct.So_Luong
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
