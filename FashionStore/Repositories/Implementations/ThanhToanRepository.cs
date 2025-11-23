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

        public async Task<ResponseMessageResult> ThanhToanCODAsync(int maKhachHang, string? maVoucher = null)
        {
            // 1. LẤY GIỎ HÀNG VÀ CHI TIẾT (ĐÚNG BIẾN THỂ)
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BienThe) // <-- ĐÚNG RỒI NÈ!!!
                        .ThenInclude(bt => bt!.SanPham)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any(x => x.So_Luong > 0))
                return _response.SetFail("Giỏ hàng trống!", 400);

            var chiTietGioHangs = gioHang.ChiTietGioHangs.Where(x => x.So_Luong > 0).ToList();
            if (!chiTietGioHangs.Any())
                return _response.SetFail("Không có sản phẩm hợp lệ!", 400);

            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var chiTietDonHangs = new List<ChiTietDonHang>();
            decimal tongTienGoc = 0;
            decimal giamGiaVoucher = 0;
            string? tenVoucher = null;

            // 2. KIỂM TRA KHO + TÍNH TIỀN CHÍNH XÁC
            foreach (var ct in chiTietGioHangs)
            {
                if (ct.BienThe == null)
                    return _response.SetFail($"Sản phẩm {ct.Ma_SanPham} không tồn tại biến thể!", 400);

                if (ct.BienThe.So_Luong < ct.So_Luong)
                    return _response.SetFail($"Sản phẩm {ct.BienThe.SanPham?.Ten_SanPham} ({ct.BienThe.Mau_Sac} - {ct.BienThe.Kich_Thuoc}) chỉ còn {ct.BienThe.So_Luong} cái!", 400);

                decimal giaBan = ct.BienThe.Gia_Giam ?? ct.BienThe.Gia_BienThe;
                decimal thanhTien = giaBan * ct.So_Luong;
                tongTienGoc += thanhTien;

                chiTietDonHangs.Add(new ChiTietDonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_SanPham = ct.Ma_SanPham,
                    Ma_BienThe = ct.BienThe.Id,
                    So_Luong = ct.So_Luong,
                    DonGia = giaBan
                });
            }

            // 3. XỬ LÝ VOUCHER – SỬA LỖI CHẾT NGƯỜI!!!
            if (!string.IsNullOrWhiteSpace(maVoucher))
            {
                var voucher = await _context.Vouchers
                    .FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher
                                           && v.Trang_Thai
                                           && v.So_LanDung > 0
                                           && v.Ngay_BatDau <= DateTime.Now
                                           && v.Ngay_KetThuc >= DateTime.Now);

                if (voucher != null && (voucher.GiaTri_ToiThieu == null || voucher.GiaTri_ToiThieu <= tongTienGoc))
                {
                    if (voucher.Giam_PhanTram.HasValue)
                    {
                        giamGiaVoucher = tongTienGoc * voucher.Giam_PhanTram.Value / 100m;
                        // NẾU CÓ GIỚI HẠN GIẢM TỐI ĐA (thêm cột Giam_ToiDa vào Voucher nếu cần)
                        // giamGiaVoucher = Math.Min(giamGiaVoucher, voucher.Giam_ToiDa ?? decimal.MaxValue);
                    }
                    else if (voucher.Giam_Tien.HasValue)
                    {
                        giamGiaVoucher = voucher.Giam_Tien.Value;
                    }

                    if (giamGiaVoucher > tongTienGoc) giamGiaVoucher = tongTienGoc; // Không âm tiền
                    tenVoucher = voucher.Ma_Voucher;
                }
            }

            decimal tongTienThanhToan = tongTienGoc - giamGiaVoucher;

            var donHang = new DonHang
            {
                Ma_DonHang = maDonHang,
                Ma_KhachHang = maKhachHang,
                Ngay_Dat = DateTime.Now,
                Tong_Tien = tongTienThanhToan,
                Trang_Thai = "Chờ xác nhận",
                Ma_PhuongThuc = 1, // COD
                Ma_Voucher = string.IsNullOrEmpty(maVoucher) ? null : maVoucher
            };

            // 4. TRANSACTION + LOCK KHO (CHỐNG ÂM KHO 100%)
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);

                // TRỪ KHO (có lock)
                foreach (var ct in chiTietGioHangs)
                {
                    ct.BienThe!.So_Luong -= ct.So_Luong;
                }

                // TRỪ VOUCHER
                if (giamGiaVoucher > 0 && !string.IsNullOrEmpty(maVoucher))
                {
                    var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher);
                    if (voucher != null) voucher.So_LanDung -= 1;
                }

                // XÓA GIỎ HÀNG
                _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
                _context.GioHangs.Remove(gioHang);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 5. GỬI EMAIL HOÀN HẢO
                var kh = await _context.KhachHangs
                    .Include(k => k.TaiKhoan)
                    .FirstOrDefaultAsync(k => k.Ma_KhachHang == maKhachHang);

                var phuongThuc = await _context.PhuongThucThanhToans
                    .FirstOrDefaultAsync(p => p.Ma_PhuongThuc == donHang.Ma_PhuongThuc);
                string tenPhuongThuc = phuongThuc?.Ten_PhuongThuc ?? "Thanh toán khi nhận hàng (COD)";

                if (kh?.TaiKhoan?.Email != null)
                {
                    var chiTietDto = chiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                    {
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "Sản phẩm",
                        Hinh_Anh = ct.SanPham?.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan ?? "/images/no-image.jpg",
                        Mau_Sac = ct.BienThe?.Mau_Sac,
                        Kich_Thuoc = ct.BienThe?.Kich_Thuoc,
                        So_Luong = ct.So_Luong,
                        DonGia = ct.DonGia,
                        ThanhTien = ct.DonGia * ct.So_Luong
                    }).ToList();

                    var donHangDto = new DonHangDTO
                    {
                        Ma_DonHang = maDonHang,
                        Ngay_Dat = donHang.Ngay_Dat,
                        Trang_Thai = donHang.Trang_Thai,
                        Ten_PhuongThuc = tenPhuongThuc,
                        Tong_Tien = tongTienThanhToan,
                        ChiTiet = chiTietDto
                    };

                    _ = Task.Run(async () => // Gửi mail không block
                    {
                        try { await _emailService.SendOrderEmailAsync(kh.TaiKhoan.Email!, donHangDto); }
                        catch { /* Không làm gì nếu lỗi mail */ }
                    });
                }

                return _response.SetSuccess("Đặt hàng COD thành công!", new
                {
                    maDonHang,
                    tongTienGoc,
                    giamGiaVoucher,
                    tenVoucher,
                    tongTienThanhToan,
                    phuongThuc = "Thanh toán khi nhận hàng (COD)",
                    ngayDat = donHang.Ngay_Dat,
                    loiNho = giamGiaVoucher > 0
                        ? $"Tiết kiệm {giamGiaVoucher:N0}đ với mã {tenVoucher}!"
                        : "Chuẩn bị tiền lẻ khi nhận hàng nhé!"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // LOG LỖI (rất quan trọng)
                Console.WriteLine($"Lỗi đặt hàng COD: {ex.Message}");
                return _response.SetFail("Đặt hàng thất bại! Vui lòng thử lại.", 500);
            }
        }
      
        public async Task<ResponseMessageResult> TaoDonHangKhiVNPAYThanhCongAsync(string maDonHang, int maKhachHang, string? maVoucher = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var gioHang = await _context.GioHangs
                    .Include(g => g.ChiTietGioHangs!)
                        .ThenInclude(ct => ct.BienThe!)
                            .ThenInclude(bt => bt!.SanPham!)
                    .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any(x => x.So_Luong > 0))
                    return _response.SetFail("Giỏ hàng trống hoặc đã bị xóa!", 400);

                // Kiểm tra tồn kho trước khi tạo đơn
                foreach (var ct in gioHang.ChiTietGioHangs.Where(x => x.So_Luong > 0))
                {
                    if (ct.BienThe!.So_Luong < ct.So_Luong)
                        return _response.SetFail($"Sản phẩm {ct.BienThe.SanPham!.Ten_SanPham} không đủ hàng!", 400);
                }

                // Tạo chi tiết đơn hàng
                var chiTietDonHangs = gioHang.ChiTietGioHangs
                    .Where(x => x.So_Luong > 0)
                    .Select(ct => new ChiTietDonHang
                    {
                        Ma_DonHang = maDonHang,
                        Ma_SanPham = ct.Ma_SanPham,
                        Ma_BienThe = ct.Ma_BienThe,
                        So_Luong = ct.So_Luong,
                        DonGia = ct.BienThe!.Gia_Giam ?? ct.BienThe.Gia_BienThe
                    }).ToList();

                // Tính tiền ban đầu
                decimal tongTienBanDau = chiTietDonHangs.Sum(x => x.So_Luong * x.DonGia);
                decimal tongTienSauGiam = tongTienBanDau;

                // Áp dụng voucher
                if (!string.IsNullOrEmpty(maVoucher))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher && v.Trang_Thai);

                    if (voucher != null)
                    {
                        var now = DateTime.Now;

                        if (voucher.Ngay_BatDau.HasValue && now < voucher.Ngay_BatDau.Value)
                            return _response.SetFail("Voucher chưa đến thời gian sử dụng!", 400);

                        if (voucher.Ngay_KetThuc.HasValue && now > voucher.Ngay_KetThuc.Value)
                            return _response.SetFail("Voucher đã hết hạn!", 400);

                        if (voucher.GiaTri_ToiThieu.HasValue && tongTienBanDau < voucher.GiaTri_ToiThieu.Value)
                            return _response.SetFail($"Đơn hàng phải từ {voucher.GiaTri_ToiThieu.Value:N0}đ mới dùng được voucher này!", 400);

                        if (voucher.So_LanDung == 0)
                            return _response.SetFail("Voucher đã hết lượt sử dụng!", 400);

                        if (voucher.Giam_PhanTram.HasValue && voucher.Giam_PhanTram > 0)
                        {
                            decimal giamPhanTram = voucher.Giam_PhanTram.Value;
                            tongTienSauGiam = tongTienBanDau - (tongTienBanDau * giamPhanTram / 100);
                        }
                        else if (voucher.Giam_Tien.HasValue && voucher.Giam_Tien > 0)
                        {
                            decimal giamTien = voucher.Giam_Tien.Value;
                            tongTienSauGiam = tongTienBanDau - Math.Min(giamTien, tongTienBanDau);
                        }

                        voucher.So_LanDung -= 1;
                    }
                }

                tongTienSauGiam = Math.Round(tongTienSauGiam, 2);

                // Tạo đơn hàng
                var donHang = new DonHang
                {
                    Ma_DonHang = maDonHang,
                    Ma_KhachHang = maKhachHang,
                    Ngay_Dat = DateTime.Now,
                    Tong_Tien = tongTienSauGiam,
                    Trang_Thai = "Đang xử lý",
                    Ma_PhuongThuc = 3, // VNPAY
                    Ma_Voucher = maVoucher
                };

                // Trừ kho
                foreach (var ct in gioHang.ChiTietGioHangs.Where(x => x.So_Luong > 0))
                {
                    ct.BienThe!.So_Luong -= ct.So_Luong;
                }

                // Xóa giỏ hàng
                _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
                _context.GioHangs.Remove(gioHang);

                // Thêm đơn hàng
                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Load email và Ten_PhuongThuc trước khi dispose context
                var kh = await _context.KhachHangs
                    .Include(k => k.TaiKhoan)
                    .FirstOrDefaultAsync(k => k.Ma_KhachHang == maKhachHang);
                string? email = kh?.TaiKhoan?.Email;

                var phuongThuc = await _context.PhuongThucThanhToans
                    .FirstOrDefaultAsync(p => p.Ma_PhuongThuc == donHang.Ma_PhuongThuc);
                string tenPhuongThuc = phuongThuc?.Ten_PhuongThuc ?? "VNPAY";

                // Tạo DonHangDTO để gửi email
                var donHangDto = new DonHangDTO
                {
                    Ma_DonHang = maDonHang,
                    Ma_KhachHang = maKhachHang,
                    Ngay_Dat = donHang.Ngay_Dat,
                    Tong_Tien = tongTienSauGiam,
                    Trang_Thai = donHang.Trang_Thai,
                    Ma_PhuongThuc = donHang.Ma_PhuongThuc,
                    Ten_PhuongThuc = tenPhuongThuc,  // Set đúng
                    Ma_Voucher = maVoucher,
                    ChiTiet = chiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                    {
                        Ma_SanPham = ct.Ma_SanPham,
                        Ten_SanPham = ct.SanPham?.Ten_SanPham ?? string.Empty,
                        Hinh_Anh = ct.SanPham?.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                        Mau_Sac = ct.BienThe?.Mau_Sac ?? string.Empty,
                        Kich_Thuoc = ct.BienThe?.Kich_Thuoc ?? string.Empty,
                        So_Luong = ct.So_Luong,
                        DonGia = ct.DonGia,
                        ThanhTien = ct.DonGia * ct.So_Luong
                    }).ToList()
                };

                // Gửi email (background) - Sử dụng email đã load
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(email))
                        {
                            await _emailService.SendOrderEmailAsync(email, donHangDto);
                        }
                    }
                    catch { /* Không crash nếu mail lỗi */ }
                });

                return _response.SetSuccess("Thanh toán thành công! Đơn hàng đã được tạo.", new { maDonHang, tongTienSauGiam });
            }
            catch (Exception )
            {
                await transaction.RollbackAsync();
                //_logger.LogError(ex, "Lỗi tạo đơn hàng sau VNPAY thành công: {MaDonHang}", maDonHang);
                return _response.SetFail("Lỗi hệ thống khi tạo đơn hàng!", 500);
            }
        }




    }
}
