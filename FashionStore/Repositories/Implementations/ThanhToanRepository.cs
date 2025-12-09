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
                // kiểm tra địa chỉ
                var diaChi = await _context.DiaChiGiaoHangs
                    .FirstOrDefaultAsync(d => d.Ma_DiaChi == maDiaChi && d.Ma_KhachHang == maKhachHang);

                if (diaChi == null)
                    return _response.SetFail("Địa chỉ giao hàng không hợp lệ!", 400);

                // lấy giỏ hàng
                var gioHang = await _context.GioHangs
                    .Include(g => g.ChiTietGioHangs!)
                        .ThenInclude(ct => ct.BienThe!)
                            .ThenInclude(bt => bt!.SanPham!)
                    .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

                if (gioHang == null)
                    return _response.SetFail("Giỏ hàng không tồn tại!", 400);

                // lấy các item đã check (hoặc nếu bạn dùng khác, điều chỉnh)
                var sanPhamThanhToan = gioHang.ChiTietGioHangs
                    .Where(x => x.IsChecked == true && x.So_Luong > 0)
                    .ToList();

                if (!sanPhamThanhToan.Any())
                    return _response.SetFail("Bạn chưa chọn sản phẩm để thanh toán!", 400);

                // kiểm tồn kho
                foreach (var ct in sanPhamThanhToan)
                {
                    if (ct.BienThe == null)
                        return _response.SetFail($"Biến thể {ct.Ma_BienThe} lỗi dữ liệu!", 400);

                    if (ct.BienThe.So_Luong < ct.So_Luong)
                        return _response.SetFail($"Sản phẩm {ct.BienThe.SanPham?.Ten_SanPham} không đủ hàng!", 400);
                }

                // chuẩn bị chi tiết đơn
                var chiTietDonHangs = sanPhamThanhToan.Select(ct => new ChiTietDonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_SanPham = ct.Ma_SanPham,
                    Ma_BienThe = ct.Ma_BienThe,
                    So_Luong = ct.So_Luong,
                    DonGia = ct.BienThe!.Gia_Giam ?? ct.BienThe.Gia_BienThe
                }).ToList();

                decimal tongTienBanDau = chiTietDonHangs.Sum(x => x.So_Luong * x.DonGia);

                // Áp voucher (nếu có)
                decimal giamGiaVoucher = 0m;
                string? tenVoucher = null;

                if (!string.IsNullOrEmpty(maVoucher))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher && v.Trang_Thai);

                    if (voucher == null)
                    {
                        // voucher không hợp lệ -> không áp dụng (bạn có thể trả fail)
                        // return _response.SetFail("Voucher không hợp lệ!", 400);
                    }
                    else
                    {
                        var now = DateTime.Now;

                        if (voucher.Ngay_BatDau.HasValue && now < voucher.Ngay_BatDau.Value)
                            return _response.SetFail("Voucher chưa đến thời gian sử dụng!", 400);

                        if (voucher.Ngay_KetThuc.HasValue && now > voucher.Ngay_KetThuc.Value)
                            return _response.SetFail("Voucher đã hết hạn!", 400);

                        if (voucher.GiaTri_ToiThieu.HasValue && tongTienBanDau < voucher.GiaTri_ToiThieu.Value)
                            return _response.SetFail($"Đơn hàng phải từ {voucher.GiaTri_ToiThieu.Value:N0}đ mới dùng được voucher này!", 400);

                        if (voucher.So_LanDung <= 0)
                            return _response.SetFail("Voucher đã hết lượt sử dụng!", 400);

                        if (voucher.Giam_PhanTram.HasValue && voucher.Giam_PhanTram.Value > 0)
                            giamGiaVoucher = tongTienBanDau * voucher.Giam_PhanTram.Value / 100m;
                        else if (voucher.Giam_Tien.HasValue && voucher.Giam_Tien.Value > 0)
                            giamGiaVoucher = Math.Min(voucher.Giam_Tien.Value, tongTienBanDau);

                        giamGiaVoucher = Math.Min(giamGiaVoucher, tongTienBanDau);
                        tenVoucher = voucher.Ma_Voucher;

                        // giảm lượt dùng
                        voucher.So_LanDung -= 1;
                        _context.Vouchers.Update(voucher);
                    }
                }

                decimal tongSauGiam = Math.Round(tongTienBanDau - giamGiaVoucher, 2);

                // tạo đơn
                var donHang = new DonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_KhachHang = maKhachHang,
                    Ma_DiaChi = maDiaChi,
                    Ngay_Dat = DateTime.Now,
                    Tong_Tien = tongSauGiam,
                    Trang_Thai = "Đang xử lý",
                    Ma_PhuongThuc = 3, // VNPAY
                    Ma_Voucher = tenVoucher
                };

                // trừ kho
                foreach (var ct in sanPhamThanhToan)
                {
                    ct.BienThe!.So_Luong -= ct.So_Luong;
                    if (ct.BienThe.So_Luong < 0) ct.BienThe.So_Luong = 0;
                    _context.SanPhamBienThes.Update(ct.BienThe);
                }

                // xóa các item đã thanh toán khỏi giỏ
                _context.ChiTietGioHangs.RemoveRange(sanPhamThanhToan);

                // reset IsChecked cho phần còn lại
                foreach (var ct in gioHang.ChiTietGioHangs)
                    ct.IsChecked = false;

                // Lưu đơn + chi tiết
                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // gửi email (không block luồng)
                var kh = await _context.KhachHangs.Include(k => k.TaiKhoan)
                    .FirstOrDefaultAsync(k => k.Ma_KhachHang == maKhachHang);
                string? email = kh?.TaiKhoan?.Email;

                if (!string.IsNullOrEmpty(email))
                {
                    var phuongThuc = await _context.PhuongThucThanhToans
                        .FirstOrDefaultAsync(p => p.Ma_PhuongThuc == donHang.Ma_PhuongThuc);

                    var donHangDto = new DonHangDTO
                    {
                        Ma_DonHang = donHang.Ma_DonHang,
                        Ma_KhachHang = donHang.Ma_KhachHang,
                        Ma_DiaChi = donHang.Ma_DiaChi,
                        Ngay_Dat = donHang.Ngay_Dat,
                        Tong_Tien = donHang.Tong_Tien,
                        Trang_Thai = donHang.Trang_Thai,
                        Ma_PhuongThuc = donHang.Ma_PhuongThuc,
                        Ten_PhuongThuc = phuongThuc?.Ten_PhuongThuc ?? "VNPAY",
                        Ma_Voucher = tenVoucher,
                        GiamGia = giamGiaVoucher,
                        ChiTiet = chiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                        {
                            Ma_SanPham = ct.Ma_SanPham,
                            Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "",
                            Hinh_Anh = ct.BienThe?.HinhAnh ?? "",
                            Mau_Sac = ct.BienThe?.Mau_Sac ?? "",
                            Kich_Thuoc = ct.BienThe?.Kich_Thuoc ?? "",
                            So_Luong = ct.So_Luong,
                            DonGia = ct.DonGia,
                            ThanhTien = ct.DonGia * ct.So_Luong
                        }).ToList()
                    };

                    _ = Task.Run(async () =>
                    {
                        try { await _emailService.SendOrderEmailAsync(email, donHangDto); }
                        catch { }
                    });
                }

                return _response.SetSuccess("Thanh toán thành công! Đơn hàng đã được tạo.", new
                {
                    maDonHang,
                    tongTienBanDau,
                    giamGia = giamGiaVoucher,
                    tongThanhToan = tongSauGiam
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return _response.SetFail("Lỗi hệ thống: " + realError, 500);
            }
        }



    }
}
