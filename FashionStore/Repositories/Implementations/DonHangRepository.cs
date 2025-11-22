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
        public async Task<ResponseMessageResult> GetAllDonHangAsync()
        {
            var donHangs = await _context.DonHangs
                 .Include(d => d.ChiTietDonHangs)
                     .ThenInclude(ct => ct.SanPham)
                         .ThenInclude(sp => sp!.HinhAnhSanPhams)
                 .Include(d => d.ChiTietDonHangs)
                     .ThenInclude(ct => ct.BienThe)
                 .Include(d => d.PhuongThucThanhToan)
                 .ToListAsync();

            var dtos = donHangs.Select(dh => new DonHangDTO
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
                    Ten_SanPham = x.SanPham!.Ten_SanPham,
                    Hinh_Anh = x.SanPham.HinhAnhSanPhams.FirstOrDefault()?.DuongDan ?? string.Empty,
                    Mau_Sac = x.BienThe?.Mau_Sac ?? string.Empty,
                    Kich_Thuoc = x.BienThe?.Kich_Thuoc ?? string.Empty,
                    So_Luong = x.So_Luong,
                    DonGia = x.DonGia,
                    ThanhTien = x.DonGia * x.So_Luong
                }).ToList()
            }).ToList();

            return _response.SetSuccess("Lấy danh sách đơn hàng thành công!", dtos);
        }
        public async Task<ResponseMessageResult> GetChiTietDonHangAsync(string maDonHang)
        {
            if (string.IsNullOrWhiteSpace(maDonHang))
                return _response.SetFail("Mã đơn hàng không hợp lệ!", 400);

            var donHang = await _context.DonHangs
                .AsNoTracking()
                .Include(d => d.KhachHang!)
                    .ThenInclude(kh => kh.TaiKhoan)
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.Voucher)
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.SanPham!)
                        .ThenInclude(sp => sp.HinhAnhSanPhams!)
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.BienThe!)
                .FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);

            if (donHang == null)
                return _response.SetFail("Không tìm thấy đơn hàng!", 404);

            var dto = new DonHangDTO
            {
                Ma_DonHang = donHang.Ma_DonHang,
                Ma_KhachHang = donHang.Ma_KhachHang,
                Ngay_Dat = donHang.Ngay_Dat,
                Tong_Tien = donHang.Tong_Tien,
                Trang_Thai = donHang.Trang_Thai,
                Ma_PhuongThuc = donHang.Ma_PhuongThuc,
                Ten_PhuongThuc = donHang.PhuongThucThanhToan?.Ten_PhuongThuc ?? "Chưa xác định",
                Ma_Voucher = donHang.Ma_Voucher,
                ChiTiet = donHang.ChiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "Sản phẩm đã xóa",
                    Hinh_Anh = ct.SanPham?.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan
                               ?? "/images/no-image.jpg",
                    Mau_Sac = ct.BienThe?.Mau_Sac ?? "Không có",
                    Kich_Thuoc = ct.BienThe?.Kich_Thuoc ?? "Freesize",
                    So_Luong = ct.So_Luong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.DonGia * ct.So_Luong
                }).ToList()
            };

            return _response.SetSuccess("Lấy chi tiết đơn hàng thành công!", dto);
        }
        public async Task<ResponseMessageResult> TaoDonHangAsync(TaoDonHangRequest request, DonHangDTO? responseDto = null)
        {
            if (request == null || request.Ma_KhachHang <= 0 || request.ChiTiet?.Any() != true)
                return _response.SetFail("Dữ liệu đơn hàng không hợp lệ!", 400);

            string maDH = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var phuongThuc = await _context.PhuongThucThanhToans
                    .FirstOrDefaultAsync(p => p.Ma_PhuongThuc == request.Ma_PhuongThuc);

                if (phuongThuc == null)
                    return _response.SetFail("Phương thức thanh toán không tồn tại!", 400);

                bool isCOD = phuongThuc.Ten_PhuongThuc.Contains("COD", StringComparison.OrdinalIgnoreCase);
                string trangThai = isCOD ? "Chờ xác nhận" : "Chờ thanh toán";

                var chiTietDonHangs = new List<ChiTietDonHang>();
                decimal tongTienGoc = 0;

                foreach (var item in request.ChiTiet)
                {
                    var bienThe = await _context.SanPhamBienThes
                        .Include(x => x.SanPham!)
                            .ThenInclude(x => x!.HinhAnhSanPhams)
                        .FirstOrDefaultAsync(x =>
                            x.Ma_SanPham == item.Ma_SanPham &&
                            x.Mau_Sac == item.Mau_Sac &&
                            x.Kich_Thuoc == item.Kich_Thuoc);

                    if (bienThe == null)
                        return _response.SetFail(
                            $"Sản phẩm {item.Ma_SanPham} màu {item.Mau_Sac} size {item.Kich_Thuoc} hiện đã hết hàng hoặc không tồn tại!", 400);

                    if (bienThe.So_Luong < item.So_Luong)
                        return _response.SetFail(
                            $"Chỉ còn {bienThe.So_Luong} sản phẩm {bienThe.SanPham?.Ten_SanPham} ({item.Mau_Sac} - {item.Kich_Thuoc})!", 400);

                    decimal giaBan = bienThe.Gia_Giam ?? bienThe.Gia_BienThe;
                    tongTienGoc += giaBan * item.So_Luong;

                    chiTietDonHangs.Add(new ChiTietDonHang
                    {
                        Ma_DonHang = maDH,
                        Ma_SanPham = item.Ma_SanPham,
                        Ma_BienThe = bienThe.Id,
                        So_Luong = item.So_Luong,
                        DonGia = giaBan
                    });

                    bienThe.So_Luong -= item.So_Luong; // trừ kho chính xác 100%
                }

                // Voucher
                decimal giamGia = 0;
                if (!string.IsNullOrWhiteSpace(request.Ma_Voucher))
                {
                    var voucher = await _context.Vouchers.FirstOrDefaultAsync(v =>
                        v.Ma_Voucher == request.Ma_Voucher && v.Trang_Thai && v.So_LanDung > 0 &&
                        v.Ngay_BatDau <= DateTime.Now && v.Ngay_KetThuc >= DateTime.Now);

                    if (voucher != null)
                    {
                        if (voucher.GiaTri_ToiThieu.HasValue && tongTienGoc < voucher.GiaTri_ToiThieu.Value)
                            return _response.SetFail($"Cần mua từ {voucher.GiaTri_ToiThieu:N0}đ để dùng mã này!", 400);

                        giamGia = voucher.Giam_PhanTram.HasValue
                            ? tongTienGoc * voucher.Giam_PhanTram.Value / 100m
                            : voucher.Giam_Tien ?? 0;

                        giamGia = Math.Min(giamGia, tongTienGoc);
                        voucher.So_LanDung--;
                    }
                }

                decimal tongCuoi = tongTienGoc - giamGia;

                var donHang = new DonHang
                {
                    Ma_DonHang = maDH,
                    Ma_KhachHang = request.Ma_KhachHang,
                    Ngay_Dat = DateTime.Now,
                    Tong_Tien = tongCuoi,
                    Trang_Thai = trangThai,
                    Ma_PhuongThuc = request.Ma_PhuongThuc,
                    Ma_Voucher = string.IsNullOrEmpty(request.Ma_Voucher) ? null : request.Ma_Voucher
                };

                _context.DonHangs.Add(donHang);
                _context.ChiTietDonHangs.AddRange(chiTietDonHangs);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Gửi mail nếu COD
                if (isCOD)
                {
                    // Tạo DTO đầy đủ TRƯỚC KHI DbContext bị dispose
                    var mailDto = new DonHangDTO
                    {
                        Ma_DonHang = maDH,
                        Ma_KhachHang = request.Ma_KhachHang,
                        Ngay_Dat = DateTime.Now,
                        Tong_Tien = tongCuoi,
                        Trang_Thai = trangThai,
                        Ten_PhuongThuc = phuongThuc.Ten_PhuongThuc,
                        Ma_Voucher = request.Ma_Voucher,
                        ChiTiet = chiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                        {
                            Ma_SanPham = ct.Ma_SanPham,
                            Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "Sản phẩm",
                            Hinh_Anh = ct.SanPham?.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan ?? "/images/no-image.jpg",
                            Mau_Sac = ct.BienThe?.Mau_Sac,
                            Kich_Thuoc = ct.BienThe?.Kich_Thuoc,
                            So_Luong = ct.So_Luong,
                            DonGia = ct.DonGia,
                            ThanhTien = ct.DonGia * ct.So_Luong
                        }).ToList()
                    };

                    // Lấy email TRƯỚC KHI DbContext chết
                    var khachHang = await _context.KhachHangs
                        .Include(k => k.TaiKhoan)
                        .FirstOrDefaultAsync(k => k.Ma_KhachHang == request.Ma_KhachHang);

                    if (!string.IsNullOrEmpty(khachHang?.TaiKhoan?.Email))
                    {
                        var email = khachHang.TaiKhoan.Email;

                        // GỬI MAIL AN TOÀN 100% – KHÔNG DÙNG Task.Run + DbContext
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await _emailService.SendOrderEmailAsync(email, mailDto);
                                //_logger.LogInformation("Gửi mail COD thành công cho khách {MaKH} - Đơn {MaDH}", request.Ma_KhachHang, maDH);
                            }
                            catch (Exception)
                            {
                                // _logger.LogError(ex, "LỖI GỬI MAIL COD - KH: {MaKH} - Đơn: {MaDH} - Email: {Email}", 
                                //request.Ma_KhachHang, maDH, email);
                            }
                        });
                    }
                }

                // Trả về DTO đầy đủ nếu frontend cần
                responseDto?.ChiTiet.Clear();
                responseDto ??= new DonHangDTO();
                responseDto.Ma_DonHang = maDH;
                responseDto.Ma_KhachHang = request.Ma_KhachHang;
                responseDto.Ma_PhuongThuc = request.Ma_PhuongThuc;
                responseDto.Ngay_Dat = donHang.Ngay_Dat;
                responseDto.Tong_Tien = tongCuoi;
                responseDto.Trang_Thai = trangThai;
                responseDto.Ten_PhuongThuc = phuongThuc.Ten_PhuongThuc;
                responseDto.ChiTiet.AddRange(chiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "Sản phẩm",
                    Hinh_Anh = ct.SanPham?.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan,
                    Mau_Sac = ct.BienThe?.Mau_Sac,
                    Kich_Thuoc = ct.BienThe?.Kich_Thuoc,
                    So_Luong = ct.So_Luong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.DonGia * ct.So_Luong
                }));

                return _response.SetSuccess("Đặt hàng thành công!", responseDto);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return _response.SetFail("Hệ thống bận, vui lòng thử lại!", 500);
            }
        }
        public async Task<ResponseMessageResult> ThongKeDonHangAsync(DateTime? fromDate, DateTime? toDate, string? groupBy)
        {
            try
            {
                // --- QUERY GỐC ---
                var query = _context.DonHangs
                    .Select(d => new
                    {
                        d.Ma_DonHang,
                        d.Ngay_Dat,
                        d.Tong_Tien,
                        TrangThai = d.Trang_Thai
                            //(d.Trang_Thai == "Chờ xác nhận" || d.Trang_Thai == "Đang xử lý")
                            //? "Đang xử lý"
                            //: d.Trang_Thai
                    })
                    .AsQueryable();

                // --- FILTER ---
                if (fromDate.HasValue)
                    query = query.Where(d => d.Ngay_Dat.Date >= fromDate.Value.Date);

                if (toDate.HasValue)
                    query = query.Where(d => d.Ngay_Dat.Date <= toDate.Value.Date);


                // --- GROUP THEO TIME ---
                List<object> thongKeTheoTime = new();

                switch (groupBy?.ToLower())
                {
                    case "day":
                        thongKeTheoTime = await query
                            .GroupBy(d => new { d.Ngay_Dat.Year, d.Ngay_Dat.Month, d.Ngay_Dat.Day, d.TrangThai })
                            .Select(g => (object)new
                            {
                                Date = $"{g.Key.Day}/{g.Key.Month}/{g.Key.Year}",
                                TrangThai = g.Key.TrangThai,
                                TongDon = g.Count(),
                                TongTien = g.Sum(x => x.Tong_Tien),
                                DonHangs = g.Select(x => new {
                                    x.Ma_DonHang,
                                    x.Ngay_Dat,
                                    x.Tong_Tien,
                                    TrangThai = x.TrangThai
                                }).ToList()
                            })
                            .ToListAsync();
                        break;

                    case "month":
                        thongKeTheoTime = await query
                            .GroupBy(d => new { d.Ngay_Dat.Year, d.Ngay_Dat.Month, d.TrangThai })
                            .Select(g => (object)new
                            {
                                Month = $"{g.Key.Month}/{g.Key.Year}",
                                TrangThai = g.Key.TrangThai,
                                TongDon = g.Count(),
                                TongTien = g.Sum(x => x.Tong_Tien),
                                DonHangs = g.Select(x => new {
                                    x.Ma_DonHang,
                                    x.Ngay_Dat,
                                    x.Tong_Tien,
                                    TrangThai = x.TrangThai
                                }).ToList()
                            })
                            .ToListAsync();
                        break;

                    case "year":
                        thongKeTheoTime = await query
                            .GroupBy(d => new { d.Ngay_Dat.Year, d.TrangThai })
                            .Select(g => (object)new
                            {
                                Year = g.Key.Year,
                                TrangThai = g.Key.TrangThai,
                                TongDon = g.Count(),
                                TongTien = g.Sum(x => x.Tong_Tien),
                                DonHangs = g.Select(x => new {
                                    x.Ma_DonHang,
                                    x.Ngay_Dat,
                                    x.Tong_Tien,
                                    TrangThai = x.TrangThai
                                }).ToList()
                            })
                            .ToListAsync();
                        break;

                    default:
                        thongKeTheoTime = new List<object>();
                        break;
                }


                // --- GROUP THEO TRẠNG THÁI ---
                var thongKeTheoTrangThai = await query
                    .GroupBy(d => d.TrangThai)
                    .Select(g => new
                    {
                        TrangThai = g.Key,
                        TongDon = g.Count(),
                        TongTien = g.Sum(x => x.Tong_Tien),

                        DonHangs = g.Select(x => new {
                            Ma_DonHang = x.Ma_DonHang,
                            x.Ngay_Dat,
                            x.Tong_Tien,
                            TrangThai = x.TrangThai
                        }).ToList()
                    })
                    .ToListAsync();

                return _response.SetSuccess("Thống kê đơn hàng thành công!", new
                {
                    ThongKeTheoTime = thongKeTheoTime,
                    ThongKeTheoTrangThai = thongKeTheoTrangThai
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Lỗi thống kê đơn hàng");
                return _response.SetFail("Lỗi thống kê: " + ex.Message, 500);
            }
        }
        public async Task<ResponseMessageResult> GetDonHangByKhachHangAsync(int maKhachHang)
        {
            try
            {
                var donHangs = await _context.DonHangs
                    .Where(d => d.Ma_KhachHang == maKhachHang)
                    .OrderByDescending(d => d.Ngay_Dat)
                    .Select(d => new
                    {
                        d.Ma_DonHang,
                        d.Ngay_Dat,
                        d.Tong_Tien,
                        d.Trang_Thai,
                        ChiTiet = d.ChiTietDonHangs.Select(ct => new
                        {
                            ct.Ma_SanPham,
                            ct.Ma_BienThe,
                            ct.So_Luong,
                            ct.DonGia,
                            Ten_SanPham = ct.SanPham!.Ten_SanPham,
                            HinhAnh = ct.SanPham.HinhAnhSanPhams.FirstOrDefault() != null
                                ? ct.SanPham.HinhAnhSanPhams.First().DuongDan
                                : "/images/no-image.jpg",
                            Mau_Sac = ct.BienThe.Mau_Sac,
                            Kich_Thuoc = ct.BienThe.Kich_Thuoc
                        }).ToList()
                    })
                    .ToListAsync();

                return _response.SetSuccess("Lấy danh sách đơn hàng thành công!", donHangs);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi khi lấy danh sách đơn hàng: " + ex.Message, 500);
            }
        }
        public async Task<ResponseMessageResult> CapNhatTrangThaiAsync(string maDonHang, string trangThaiMoi)
        {
            try
            {
                var donHang = await _context.DonHangs
                    .FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);

                if (donHang == null)
                    return _response.SetFail("Đơn hàng không tồn tại!", 404);

                // Kiểm tra trạng thái hiện tại, ví dụ:
                // Không cho cập nhật nếu đơn đã giao hoặc đã hủy
                if (donHang.Trang_Thai == "Đã giao" || donHang.Trang_Thai == "Đã hủy")
                    return _response.SetFail($"Không thể cập nhật trạng thái đơn hàng đã '{donHang.Trang_Thai}'!", 400);

                donHang.Trang_Thai = trangThaiMoi;
                await _context.SaveChangesAsync();

                return _response.SetSuccess("Cập nhật trạng thái thành công!", new
                {
                    donHang.Ma_DonHang,
                    donHang.Trang_Thai
                });
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi cập nhật trạng thái: " + ex.Message, 500);
            }
        }

    }

    public class TaoDonHangRequest
    {
        public int Ma_KhachHang { get; set; }
        public int Ma_PhuongThuc { get; set; }
        public string? Ma_Voucher { get; set; }
        public List<TaoChiTietDonHangItem> ChiTiet { get; set; } = new();
    }

    public class TaoChiTietDonHangItem
    {
        public string Ma_SanPham { get; set; } = null!;
        public int? Ma_BienThe { get; set; } // null = lấy biến thể mặc định
        public string? Mau_Sac { get; set; }
        public string? Kich_Thuoc { get; set; }
        public int So_Luong { get; set; }
    }
}
