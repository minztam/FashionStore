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
                    .ThenInclude(ct => ct.SanPham!)
                        .ThenInclude(sp => sp.HinhAnhSanPhams)
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BienThe)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetSuccess("Giỏ hàng trống!", new GioHangDTO
                {
                    Ma_GioHang = 0,
                    Ma_KhachHang = maKhachHang,
                    SanPhams = new List<ChiTietGioHangDTO>()
                });

            // Nếu không có giỏ hàng → trả về giỏ trống (không lỗi)
            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                return _response.SetSuccess("Giỏ hàng trống!", new GioHangDTO
                {
                    Ma_GioHang = 0,
                    Ma_KhachHang = maKhachHang,
                    SanPhams = new List<ChiTietGioHangDTO>()
                });
            }

            // Nếu không có giỏ hàng → trả về giỏ trống (không lỗi)
            var sanPhamsDto = gioHang.ChiTietGioHangs
                .Where(ct => ct.So_Luong > 0 && ct.BienThe != null && ct.SanPham != null)
                .Select(ct => new ChiTietGioHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                    Hinh_Anh = ct.BienThe?.HinhAnh ?? string.Empty,
                    Mau_Sac = ct.BienThe?.Mau_Sac ?? string.Empty,
                    Kich_Thuoc = ct.BienThe?.Kich_Thuoc ?? string.Empty,
                    Gia_Goc = ct.BienThe!.Gia_BienThe,
                    Gia_Giam = ct.BienThe.Gia_Giam,
                    So_Luong = ct.So_Luong,
                    Ma_BienThe = ct.Ma_BienThe,
                    InvalidReason = ct.InvalidReason,
                    IsInvalid=ct.IsInvalid,
                    
                   
                })
                .OrderBy(x => x.Ten_SanPham)
                .ToList();

            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = sanPhamsDto
            };

            return _response.SetSuccess("Lấy giỏ hàng thành công!", gioHangDto);
        }

        public async Task<ResponseMessageResult> AddToCartAsync(int maKhachHang, string maSanPham, int soLuong, int maBienThe)
        {
            if (soLuong <= 0)
                return _response.SetFail("Số lượng phải lớn hơn 0!");
            // Tìm giỏ hàng của khách hàng, bao gồm chi tiết
            var gioHang = await _context.GioHangs
                  .Include(g => g.ChiTietGioHangs)
                  .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);
            // Nếu chưa có giỏ hàng, tạo mới
            if (gioHang == null)
            {
                gioHang = new GioHang { Ma_KhachHang = maKhachHang };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }
            // Kiểm tra sản phẩm tồn tại
            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham == null)
                return _response.SetFail("Sản phẩm không tồn tại", 404);

            // Kiểm tra biến thể tồn tại và thuộc sản phẩm
            var bienThe = await _context.SanPhamBienThes.FindAsync(maBienThe);
            if (bienThe == null || bienThe.Ma_SanPham != maSanPham)
                return _response.SetFail("Biến thể sản phẩm không tồn tại", 404);

            if (bienThe.So_Luong < soLuong)
                return _response.SetFail($"Chỉ còn {bienThe.So_Luong} sản phẩm trong kho!", 400);

            // Tìm chi tiết giỏ hàng đã có với Ma_SanPham và Ma_BienThe
            var existing = gioHang.ChiTietGioHangs.FirstOrDefault(x => x.Ma_SanPham == maSanPham && x.Ma_BienThe == maBienThe);
            if (existing != null)
            {
                // Nếu đã có, tăng số lượng
                existing.So_Luong += soLuong;
            }
            else
            {
                // Nếu chưa có, thêm mới
                gioHang.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    Ma_GioHang = gioHang.Ma_GioHang,
                    Ma_SanPham = maSanPham,
                    Ma_BienThe = maBienThe,
                    So_Luong = soLuong
                });
            }
            // Lưu thay đổi
            await _context.SaveChangesAsync();
            // Reload giỏ hàng với thông tin đầy đủ (sản phẩm, hình ảnh, biến thể)
            gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp!.HinhAnhSanPhams)
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.BienThe)
                .FirstOrDefaultAsync(g => g.Ma_GioHang == gioHang.Ma_GioHang);
            // Map sang GioHangDTO
            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang!.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs.Select(ct =>
                {
                    var bienTheCt = ct.BienThe!;
                    decimal giaGoc = bienTheCt.Gia_BienThe;
                    decimal? giaGiam = bienTheCt.Gia_Giam;
                    return new ChiTietGioHangDTO
                    {
                        Ma_SanPham = ct.Ma_SanPham,
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                        Hinh_Anh = ct.BienThe?.HinhAnh ?? string.Empty,
                        Mau_Sac = bienTheCt.Mau_Sac,
                        Kich_Thuoc = bienTheCt.Kich_Thuoc,
                        Gia_Goc = giaGoc,
                        Gia_Giam = giaGiam,
                        So_Luong = ct.So_Luong,
                        Ma_BienThe = ct.Ma_BienThe
                    };
                }).ToList()
            };
            return _response.SetSuccess("Thêm vào giỏ hàng thành công", gioHangDto);
        }

        public async Task<ResponseMessageResult> UpdateCartAsync(int maKhachHang, string maSanPham, int soLuong, int maBienThe)
        {
            // 1. Lấy giỏ hàng + chi tiết đầy đủ
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.SanPham!)
                        .ThenInclude(sp => sp.HinhAnhSanPhams)
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BienThe)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetFail("Giỏ hàng không tồn tại!", 404);

            // 2. Tìm đúng item theo Ma_SanPham + Ma_BienThe
            var item = gioHang.ChiTietGioHangs
                .FirstOrDefault(x => x.Ma_SanPham == maSanPham && x.Ma_BienThe == maBienThe);

            if (item == null)
                return _response.SetFail("Sản phẩm này (màu/size đã chọn) không có trong giỏ hàng!", 404);

            // 3. Nếu số lượng <= 0 → XÓA KHỎI GIỎ
            if (soLuong <= 0)
            {
                _context.ChiTietGioHangs.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                // 4. KIỂM TRA TỒN KHO TRƯỚC KHI CẬP NHẬT
                if (item.BienThe == null || item.BienThe.So_Luong < soLuong)
                {
                    return _response.SetFail(
                        $"Chỉ còn {item.BienThe?.So_Luong ?? 0} sản phẩm trong kho! Không thể cập nhật.",
                        400);
                }

                item.So_Luong = soLuong;
                await _context.SaveChangesAsync();
            }

            // 5. Tạo DTO trả về – ĐẸP LUNG LINH, ĐÚNG MÀU, ĐÚNG SIZE, CÓ THÀNH TIỀN
            var gioHangDto = new GioHangDTO
            {
                Ma_GioHang = gioHang!.Ma_GioHang,
                Ma_KhachHang = gioHang.Ma_KhachHang,
                SanPhams = gioHang.ChiTietGioHangs.Select(ct =>
                {
                    var bienTheCt = ct.BienThe!;
                    decimal giaGoc = bienTheCt.Gia_BienThe;
                    decimal? giaGiam = bienTheCt.Gia_Giam;
                    return new ChiTietGioHangDTO
                    {
                        Ma_SanPham = ct.Ma_SanPham,
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                        Hinh_Anh = ct.BienThe?.HinhAnh ?? string.Empty,
                        Mau_Sac = bienTheCt.Mau_Sac,
                        Kich_Thuoc = bienTheCt.Kich_Thuoc,
                        Gia_Goc = giaGoc,
                        Gia_Giam = giaGiam,
                        So_Luong = ct.So_Luong,
                        Ma_BienThe = ct.Ma_BienThe
                    };
                }).ToList()
            };

            // Nếu giỏ rỗng sau khi xóa
            if (!gioHangDto.SanPhams.Any())
                return _response.SetSuccess("Giỏ hàng đã trống!", gioHangDto);

            return _response.SetSuccess("Cập nhật giỏ hàng thành công!", gioHangDto);
        }

        public async Task<ResponseMessageResult> RemoveFromCartAsync(int maKhachHang, string maSanPham, int maBienThe)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null)
                return _response.SetFail("Giỏ hàng không tồn tại!", 404);

            // QUAN TRỌNG: TÌM ĐÚNG ITEM THEO CẢ MÀU + SIZE
            var item = gioHang.ChiTietGioHangs
                .FirstOrDefault(x => x.Ma_SanPham == maSanPham && x.Ma_BienThe == maBienThe);

            if (item == null)
                return _response.SetFail("Sản phẩm này (màu/size đã chọn) không có trong giỏ hàng!", 404);

            _context.ChiTietGioHangs.Remove(item);
            await _context.SaveChangesAsync();

            // Trả về giỏ hàng cập nhật 
            return await GetCartAsync(maKhachHang);
        }
    }
}
