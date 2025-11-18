using Azure;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;
        private readonly EmailService _emailService;

        public DonHangRepository(FashionStoreContext context, ResponseMessageResult response, EmailService emailService) 
        {
            _context = context;
            _response = response;
            _emailService = emailService;
        }
        public async Task<ResponseMessageResult> GetDonHang(string maDH)
        {
            var dh = await _context.DonHangs
        .Include(d => d.ChiTietDonHangs)
        .Include(d => d.PhuongThucThanhToan)
        .FirstOrDefaultAsync(d => d.Ma_DonHang == maDH);

            if (dh == null)
                return _response.SetFail("Không tìm thấy đơn hàng!");

            var dto = new DonHangDTO
            {
                Ma_DonHang = dh.Ma_DonHang,
                Ma_KhachHang = dh.Ma_KhachHang,
                Ngay_Dat = dh.Ngay_Dat,
                Tong_Tien = dh.Tong_Tien,
                Trang_Thai = dh.Trang_Thai,
                Ma_PhuongThuc = dh.Ma_PhuongThuc,
                Ten_PhuongThuc = dh.PhuongThucThanhToan?.Ten_PhuongThuc,
                Ma_Voucher = dh.Ma_Voucher,
                ChiTiet = dh.ChiTietDonHangs.Select(x => new ChiTietDonHangDTO
                {
                    Ma_SanPham = x.Ma_SanPham,
                    Ten_SanPham = x.SanPham?.Ten_SanPham,
                    So_Luong = x.So_Luong,
                    DonGia = x.DonGia
                }).ToList()
            };

            return _response.SetSuccess("Lấy đơn hàng thành công!", dto);
        }

        public async Task<ResponseMessageResult> TaoDonHangAsync(DonHangDTO dto)
        {
            try
            {
                // Tạo mã đơn hàng
                string maDH = "DH" + DateTime.Now.Ticks;

                // Lấy thông tin phương thức thanh toán
                var phuongThuc = await _context.PhuongThucThanhToans
                    .FirstOrDefaultAsync(p => p.Ma_PhuongThuc == dto.Ma_PhuongThuc);

                if (phuongThuc == null)
                    return _response.SetFail("Phương thức thanh toán không hợp lệ!");

                // Set trạng thái ban đầu
                string trangThai = "Đang xử lý"; // mặc định COD
                bool guiMailNgay = true; // COD gửi mail ngay

                if (phuongThuc.Ten_PhuongThuc.ToLower() == "Ví điện tử" || phuongThuc.Ten_PhuongThuc.ToLower() == "Ngân hàng")
                {
                    trangThai = "Chờ thanh toán";
                    guiMailNgay = false; // mail gửi sau khi thanh toán thành công
                }

                // Tạo đơn hàng
                var donhang = new DonHang
                {
                    Ma_DonHang = maDH,
                    Ma_KhachHang = dto.Ma_KhachHang,
                    Ma_PhuongThuc = dto.Ma_PhuongThuc,
                    Ma_Voucher = dto.Ma_Voucher,
                    Trang_Thai = trangThai,
                    Ngay_Dat = DateTime.Now
                };

                // Thêm chi tiết đơn hàng
                decimal tongTien = 0;
                foreach (var item in dto.ChiTiet)
                {
                    var ct = new ChiTietDonHang
                    {
                        Ma_DonHang = maDH,
                        Ma_SanPham = item.Ma_SanPham,
                        So_Luong = item.So_Luong,
                        DonGia = item.DonGia
                    };
                    tongTien += item.So_Luong * item.DonGia;
                    _context.ChiTietDonHangs.Add(ct);
                }

                // Áp dụng voucher
                if (!string.IsNullOrEmpty(dto.Ma_Voucher))
                {
                    var voucher = await _context.Vouchers.FindAsync(dto.Ma_Voucher);
                    if (voucher != null)
                    {
                        if (voucher.GiaTri_ToiThieu != null && tongTien < voucher.GiaTri_ToiThieu)
                            return _response.SetFail($"Đơn phải đạt tối thiểu {voucher.GiaTri_ToiThieu} để dùng voucher!");

                        if (voucher.Giam_PhanTram != null)
                            tongTien -= tongTien * voucher.Giam_PhanTram.Value / 100;

                        if (voucher.Giam_Tien != null)
                            tongTien -= voucher.Giam_Tien.Value;
                    }
                }

                donhang.Tong_Tien = tongTien;
                _context.DonHangs.Add(donhang);
                await _context.SaveChangesAsync();

                // Map DTO trả về
                dto.Ma_DonHang = maDH;
                dto.Trang_Thai = donhang.Trang_Thai;
                dto.Ngay_Dat = donhang.Ngay_Dat;
                dto.Tong_Tien = tongTien;

                // Lấy email khách hàng
                var khachHang = await _context.KhachHangs
                    .Include(k => k.TaiKhoan)
                    .FirstOrDefaultAsync(k => k.Ma_KhachHang == dto.Ma_KhachHang);

                string emailKhachHang = khachHang?.TaiKhoan?.Email ?? string.Empty;

                // Gửi mail nếu COD
                if (guiMailNgay && !string.IsNullOrEmpty(emailKhachHang))
                {
                    await _emailService.SendOrderEmailAsync(emailKhachHang, dto);
                }

                return _response.SetSuccess("Tạo đơn hàng thành công!", dto);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi tạo đơn hàng: " + ex.Message, 500);
            }
        }

    }
}
