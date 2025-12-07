using Azure.Core;
using FashionStore.Controllers;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Models.VNPay;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using FashionStore.Services.VnPay;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using static FashionStore.Controllers.ThanhToanController;

namespace FashionStore.Repositories.Implementations
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;
        private readonly VnPayService _vnPayService;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _httpContext;

        public ThanhToanRepository(FashionStoreContext context, ResponseMessageResult response, EmailService emailService, VnPayService vnPayService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _response = response;
            _emailService = emailService;
            _vnPayService = vnPayService;
            _httpContext = httpContext;
        }

        public async Task<ResponseMessageResult> ThanhToanCODAsync(ThanhToanCODRequest request)
        {
            if (request == null || request.Ma_KhachHang <= 0 || !request.ChiTiet.Any())
                return _response.SetFail("Dữ liệu không hợp lệ!", 400);

            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // Lấy giỏ hàng khách
                var gioHang = await _context.GioHangs
                    .Include(g => g.ChiTietGioHangs!)
                        .ThenInclude(ct => ct.BienThe!)
                            .ThenInclude(bt => bt!.SanPham!)
                                .ThenInclude(sp => sp.HinhAnhSanPhams)
                    .FirstOrDefaultAsync(g => g.Ma_KhachHang == request.Ma_KhachHang);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                    return _response.SetFail("Giỏ hàng trống!", 400);

                // Lọc chỉ những item được chọn thanh toán
                var selectedItems = gioHang.ChiTietGioHangs
                    .Where(x => request.ChiTiet.Select(c => c.Ma_BienThe).Contains(x.Ma_BienThe))
                    .ToList();

                if (!selectedItems.Any())
                    return _response.SetFail("Không có sản phẩm hợp lệ để thanh toán!", 400);

                // Group theo biến thể, cộng dồn số lượng nếu trùng
                var listCartItems = selectedItems
                    .GroupBy(x => x.Ma_BienThe)
                    .Select(g => new
                    {
                        EntityGoc = g.First(),
                        BienThe = g.First().BienThe,
                        Ma_SanPham = g.First().Ma_SanPham,
                        Ma_BienThe = g.Key,
                        So_Luong = request.ChiTiet.First(c => c.Ma_BienThe == g.Key).So_Luong
                    })
                    .ToList();

                decimal tongTienGoc = 0;
                var chiTietDonHangs = new List<ChiTietDonHang>();
                var emailDetails = new List<ChiTietDonHangDTO>();

                // Tính tiền & trừ kho
                foreach (var item in listCartItems)
                {
                    if (item.BienThe == null)
                        return _response.SetFail($"Sản phẩm ID {item.Ma_SanPham} bị lỗi dữ liệu!", 400);

                    if (item.BienThe.So_Luong < item.So_Luong)
                        return _response.SetFail($"Sản phẩm '{item.BienThe.SanPham?.Ten_SanPham}' chỉ còn {item.BienThe.So_Luong}!", 400);

                    decimal giaBan = item.BienThe.Gia_Giam ?? item.BienThe.Gia_BienThe;
                    tongTienGoc += giaBan * item.So_Luong;

                    chiTietDonHangs.Add(new ChiTietDonHang
                    {
                        Ma_DonHang = maDonHang,
                        Ma_SanPham = item.Ma_SanPham,
                        Ma_BienThe = item.Ma_BienThe,
                        So_Luong = item.So_Luong,
                        DonGia = giaBan
                    });

                    // Trừ kho
                    item.BienThe.So_Luong -= item.So_Luong;

                    emailDetails.Add(new ChiTietDonHangDTO
                    {
                        Ma_SanPham = item.Ma_SanPham,
                        Ten_SanPham = item.BienThe.SanPham?.Ten_SanPham ?? "Sản phẩm",
                        Mau_Sac = item.BienThe.Mau_Sac,
                        Kich_Thuoc = item.BienThe.Kich_Thuoc,
                        So_Luong = item.So_Luong,
                        DonGia = giaBan,
                        ThanhTien = giaBan * item.So_Luong,
                        Hinh_Anh = item.BienThe.HinhAnh ?? "/images/no-image.jpg"
                    });
                }

                // Voucher
                decimal giamGiaVoucher = 0;
                string? tenVoucher = null;
                if (!string.IsNullOrEmpty(request.Ma_Voucher))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Ma_Voucher == request.Ma_Voucher && v.Trang_Thai && v.So_LanDung > 0);

                    if (voucher != null && (voucher.GiaTri_ToiThieu == null || voucher.GiaTri_ToiThieu <= tongTienGoc))
                    {
                        giamGiaVoucher = voucher.Giam_PhanTram.HasValue
                            ? tongTienGoc * voucher.Giam_PhanTram.Value / 100m
                            : Math.Min(voucher.Giam_Tien ?? 0, tongTienGoc);

                        giamGiaVoucher = Math.Min(giamGiaVoucher, tongTienGoc);
                        tenVoucher = voucher.Ma_Voucher;
                        voucher.So_LanDung--;
                    }
                }

                decimal tongThanhToan = tongTienGoc - giamGiaVoucher;

                // Tạo đơn hàng
                var donHang = new DonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_KhachHang = request.Ma_KhachHang,
                    Ma_DiaChi = request.Ma_DiaChi,
                    Ngay_Dat = DateTime.Now,
                    Tong_Tien = tongThanhToan,
                    Trang_Thai = "Chờ xác nhận",
                    Ma_PhuongThuc = 1, // COD
                    
                    Ma_Voucher = tenVoucher,
                };

                // Xóa chỉ các item đã thanh toán
                _context.ChiTietGioHangs.RemoveRange(selectedItems);

                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Gửi email (background)
                var emailKhachHang = await _context.KhachHangs
                    .Where(k => k.Ma_KhachHang == request.Ma_KhachHang)
                    .Select(k => k.TaiKhoan != null ? k.TaiKhoan.Email : null)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(emailKhachHang))
                {
                    var donHangDto = new DonHangDTO
                    {
                        Ma_DonHang = maDonHang,
                        Ma_DiaChi = request.Ma_DiaChi,
                        Ngay_Dat = donHang.Ngay_Dat,
                        Tong_Tien = tongThanhToan,
                        Trang_Thai = donHang.Trang_Thai,
                        Ten_PhuongThuc = "COD",
                        Ma_Voucher = tenVoucher,
                        ChiTiet = emailDetails
                    };

                    _ = Task.Run(async () =>
                    {
                        try { await _emailService.SendOrderEmailAsync(emailKhachHang, donHangDto); }
                        catch { }
                    });
                }

                return _response.SetSuccess("Đặt hàng thành công!", new
                {
                    maDonHang,
                    tongTienGoc,
                    giamGia = giamGiaVoucher,
                    tongThanhToan
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"[Lỗi Đặt Hàng]: {realError}");
                return _response.SetFail("Đặt hàng thất bại: " + realError, 500);
            }
        }

        public async Task<ResponseMessageResult> TaoDonHangKhiVNPAYThanhCongAsync(
    string maDonHang,
    int maKhachHang,
    int maDiaChi,
    string? maVoucher = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // Kiểm tra địa chỉ
                var diaChi = await _context.DiaChiGiaoHangs
                    .FirstOrDefaultAsync(d => d.Ma_DiaChi == maDiaChi && d.Ma_KhachHang == maKhachHang);
                if (diaChi == null)
                    return _response.SetFail("Địa chỉ giao hàng không hợp lệ!", 400);

                // LẤY GIỎ HÀNG CỦA KHÁCH – CHỈ LẤY NHỮNG SẢN PHẨM CÓ SỐ LƯỢNG > 0
                var gioHang = await _context.GioHangs
                    .Include(g => g.ChiTietGioHangs!)
                        .ThenInclude(ct => ct.BienThe!)
                            .ThenInclude(bt => bt!.SanPham!)
                                .ThenInclude(sp => sp.HinhAnhSanPhams)
                    .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

                

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any(ct => ct.So_Luong > 0))
                    return _response.SetFail("Giỏ hàng trống hoặc không có sản phẩm nào được chọn!", 400);

                // LỌC CHỈ NHỮNG SẢN PHẨM CÓ SỐ LƯỢNG > 0 → ĐÚNG NHỮNG MÓN KHÁCH ĐÃ CHỌN
                var selectedItems = await _context.ChiTietGioHangs.Where(x => x.IsChecked && x.GioHang.Ma_KhachHang == maKhachHang).ToListAsync();

                if (!selectedItems.Any())
                    return _response.SetFail("Không có sản phẩm nào được chọn để thanh toán!", 400);

                // Group theo biến thể (giống hệt COD)
                var listCartItems = selectedItems
                    .GroupBy(x => x.Ma_BienThe)
                    .Select(g => new
                    {
                        EntityGoc = g.First(),
                        BienThe = g.First().BienThe,
                        Ma_SanPham = g.First().Ma_SanPham,
                        Ma_BienThe = g.Key,
                        So_Luong = g.Sum(x => x.So_Luong) // Cộng dồn nếu trùng biến thể
                    })
                    .ToList();

                decimal tongTienGoc = 0;
                var chiTietDonHangs = new List<ChiTietDonHang>();
                var emailDetails = new List<ChiTietDonHangDTO>();

                foreach (var item in listCartItems)
                {
                    if (item.BienThe == null)
                        return _response.SetFail($"Sản phẩm ID {item.Ma_SanPham} bị lỗi dữ liệu!", 400);
                    if (item.BienThe.So_Luong < item.So_Luong)
                        return _response.SetFail($"Sản phẩm '{item.BienThe.SanPham?.Ten_SanPham}' chỉ còn {item.BienThe.So_Luong}!", 400);

                    decimal giaBan = item.BienThe.Gia_Giam ?? item.BienThe.Gia_BienThe;
                    tongTienGoc += giaBan * item.So_Luong;

                    chiTietDonHangs.Add(new ChiTietDonHang
                    {
                        Ma_DonHang = maDonHang,
                        Ma_SanPham = item.Ma_SanPham,
                        Ma_BienThe = item.Ma_BienThe,
                        So_Luong = item.So_Luong,
                        DonGia = giaBan
                    });

                    item.BienThe.So_Luong -= item.So_Luong;

                    emailDetails.Add(new ChiTietDonHangDTO
                    {
                        Ma_SanPham = item.Ma_SanPham,
                        Ten_SanPham = item.BienThe.SanPham?.Ten_SanPham ?? "Sản phẩm",
                        Mau_Sac = item.BienThe.Mau_Sac,
                        Kich_Thuoc = item.BienThe.Kich_Thuoc,
                        So_Luong = item.So_Luong,
                        DonGia = giaBan,
                        ThanhTien = giaBan * item.So_Luong,
                        Hinh_Anh = item.BienThe.HinhAnh ?? "/images/no-image.jpg"
                    });
                }

                // Áp voucher
                decimal giamGiaVoucher = 0;
                string? tenVoucher = null;
                if (!string.IsNullOrEmpty(maVoucher))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher && v.Trang_Thai && v.So_LanDung > 0);
                    if (voucher != null && (voucher.GiaTri_ToiThieu == null || voucher.GiaTri_ToiThieu <= tongTienGoc))
                    {
                        giamGiaVoucher = voucher.Giam_PhanTram.HasValue
                            ? tongTienGoc * voucher.Giam_PhanTram.Value / 100m
                            : Math.Min(voucher.Giam_Tien ?? 0, tongTienGoc);
                        giamGiaVoucher = Math.Min(giamGiaVoucher, tongTienGoc);
                        tenVoucher = voucher.Ma_Voucher;
                        voucher.So_LanDung--;
                    }
                }

                decimal tongThanhToan = tongTienGoc - giamGiaVoucher;

                var donHang = new DonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_KhachHang = maKhachHang,
                    Ma_DiaChi = maDiaChi,
                    Ngay_Dat = DateTime.Now,
                    Tong_Tien = tongThanhToan,
                    Trang_Thai = "Đã thanh toán",
                    Ma_PhuongThuc = 3,
                    Ma_Voucher = tenVoucher
                };

                // XÓA CHỈ NHỮNG SẢN PHẨM ĐÃ CHỌN (có So_Luong > 0)
                _context.ChiTietGioHangs.RemoveRange(selectedItems);

                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Gửi email
                var emailKhachHang = await _context.KhachHangs
                    .Where(k => k.Ma_KhachHang == maKhachHang)
                    .Select(k => k.TaiKhoan != null ? k.TaiKhoan.Email : null)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(emailKhachHang))
                {
                    var donHangDto = new DonHangDTO
                    {
                        Ma_DonHang = maDonHang,
                        Ngay_Dat = donHang.Ngay_Dat,
                        Tong_Tien = tongThanhToan,
                        Trang_Thai = "Đã thanh toán thành công",
                        Ten_PhuongThuc = "Thanh toán qua VNPAY",
                        ChiTiet = emailDetails
                    };
                    _ = Task.Run(async () => await _emailService.SendOrderEmailAsync(emailKhachHang, donHangDto));
                }

                return _response.SetSuccess("Thanh toán VNPAY thành công!", new
                {
                    maDonHang,
                    tongTienGoc,
                    giamGia = giamGiaVoucher,
                    tongThanhToan
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return _response.SetFail("Lỗi hệ thống khi tạo đơn hàng!", 500);
            }
        }

      
    }
}
