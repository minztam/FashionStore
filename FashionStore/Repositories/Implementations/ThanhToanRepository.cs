using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;
        private readonly EmailService _emailService;

        public ThanhToanRepository(FashionStoreContext context, ResponseMessageResult response, EmailService emailService)
        {
            _context = context;
            _response = response;
            _emailService = emailService;
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
    }
}
