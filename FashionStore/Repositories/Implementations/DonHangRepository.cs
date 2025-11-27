using Azure;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text;

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
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.BienThe)
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.DiaChiGiaoHang) // <-- include DiaChi
                .ToListAsync();

            var dtos = donHangs.Select(dh => new DonHangDTO
            {
                Ma_DonHang = dh.Ma_DonHang,
                Ma_KhachHang = dh.Ma_KhachHang,
                Ngay_Dat = dh.Ngay_Dat,
                Tong_Tien = dh.Tong_Tien,
                Trang_Thai = dh.Trang_Thai,
                Ten_PhuongThuc = dh.PhuongThucThanhToan?.Ten_PhuongThuc,
                Ma_Voucher = dh.Ma_Voucher,
                DiaChi = dh.DiaChiGiaoHang ,
                Ma_Shipper = dh.Ma_Shipper,
                ChiTiet = dh.ChiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham!.Ten_SanPham,
                    Mau_Sac = ct.BienThe?.Mau_Sac ?? string.Empty,
                    Kich_Thuoc = ct.BienThe?.Kich_Thuoc ?? string.Empty,
                    Hinh_Anh=ct.BienThe?.HinhAnh ?? string.Empty,
                    So_Luong = ct.So_Luong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.DonGia * ct.So_Luong
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
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.BienThe!).Include(d => d.DiaChiGiaoHang)
                .FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);

            if (donHang == null)
                return _response.SetFail("Không tìm thấy đơn hàng!", 404);

            var dto = new DonHangDTO
            {
                Ma_DonHang = donHang.Ma_DonHang,
                Ma_KhachHang = donHang.Ma_KhachHang,
                Ma_DiaChi=donHang.Ma_DiaChi,
                Ngay_Dat = donHang.Ngay_Dat,
                Tong_Tien = donHang.Tong_Tien,
                Trang_Thai = donHang.Trang_Thai,
                Ten_PhuongThuc = donHang.PhuongThucThanhToan?.Ten_PhuongThuc ?? "Chưa xác định",
                Ma_Voucher = donHang.Ma_Voucher,
                DiaChi=donHang.DiaChiGiaoHang,
                ChiTiet = donHang.ChiTietDonHangs.Select(ct => new ChiTietDonHangDTO
                {
                    Ma_SanPham = ct.Ma_SanPham,
                    Ten_SanPham = ct.SanPham?.Ten_SanPham ?? "Sản phẩm đã xóa",
                    Hinh_Anh = ct.BienThe?.HinhAnh??"không có hình ảnh",
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
                    Ma_DiaChi =request.Ma_DiaChi,
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
                            Hinh_Anh = ct.BienThe?.HinhAnh?? "/images/no-image.jpg",
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
                    Hinh_Anh = ct.BienThe?.HinhAnh,
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
                      .Include(d => d.DiaChiGiaoHang)
                    .Select(d => new
                    {
                        d.Ma_DonHang,
                        d.Ngay_Dat,
                        d.Tong_Tien,
                        d.Trang_Thai,
                        d.DiaChiGiaoHang,
                        ChiTiet = d.ChiTietDonHangs.Select(ct => new
                        {
                            ct.Ma_SanPham,
                            ct.Ma_BienThe,
                            ct.So_Luong,
                            ct.DonGia,
                            Ten_SanPham = ct.SanPham!.Ten_SanPham,
                            HinhAnh = ct.BienThe.HinhAnh
                                ?? "/images/no-image.jpg",
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

        public async Task<DonHang?> GetDonHangForInvoiceAsync(string maDonHang)
        {
            return await _context.DonHangs
                .AsNoTracking()
                .Include(d => d.KhachHang!).ThenInclude(k => k!.TaiKhoan)
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.SanPham!)
                        .ThenInclude(sp => sp!.HinhAnhSanPhams!)
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.BienThe!)
                .FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);
        }
        public async Task<ResponseMessageResult> GanDonHangChoShipperAsync( string maDonHang)
        {
            // 1️⃣ Lấy đơn hàng
            var donHang = await _context.DonHangs.FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);
            if (donHang == null)
                return _response.SetFail("Đơn hàng không tồn tại", 404);

            // 2️⃣ Lấy danh sách shipper khả dụng
            var availableShippers = await _context.Shippers
                .Where(s => s.TrangThai == "online") // trạng thái shipper khả dụng
                .ToListAsync();

            if (!availableShippers.Any())
                return _response.SetFail("Hiện không có shipper khả dụng", 404);

            // 3️⃣ Random 1 shipper từ danh sách khả dụng
            var random = new Random();
            var shipper = availableShippers[random.Next(availableShippers.Count)];

            // 4️⃣ Gán shipper vào đơn hàng
            donHang.Ma_Shipper = shipper.Ma_Shipper;
            donHang.Trang_Thai = "Đang chờ shipper tới nhận hàng";

            await _context.SaveChangesAsync();

            return _response.SetSuccess($"Đơn hàng {maDonHang} đã được gán cho shipper {shipper.Ten_DayDu}, biển số xe shipper là {shipper.BienSoXe} với số điện thoại là {shipper.SoDienThoai}");

        }


        public async Task<string> GenerateInvoiceHtmlAsync(string maDonHang)
        {
            var donHang = await GetDonHangForInvoiceAsync(maDonHang);
            if (donHang == null)
                return "<h1 style='text-align:center;color:red;margin-top:100px;font-family:Arial'>KHÔNG TÌM THẤY ĐƠN HÀNG!</h1>";

            var sb = new StringBuilder();
            sb.Append(@"
<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""utf-8"">
    <title>Hóa đơn - ").Append(maDonHang).Append(@"</title>
    <style>
        body { 
            font-family: 'DejaVu Sans', Arial, sans-serif; 
            margin: 0; 
            padding: 10mm; 
            font-size: 11pt; 
            line-height: 1.4;
            color: #000;
        }
        .invoice {
            width: 100%;
            max-width: 800px;
            margin: 0 auto;
            border: 2px solid #333;
            padding: 20px;
            background: white;
        }
        .header { 
            text-align: center; 
            border-bottom: 3px double #333; 
            padding-bottom: 15px; 
            margin-bottom: 20px;
        }
        .header h1 { 
            margin: 0; 
            font-size: 28pt; 
            color: #d32f2f; 
            font-weight: bold;
        }
        .header p { margin: 5px 0; font-size: 12pt; }
        .info { 
            margin: 20px 0; 
            font-size: 12pt;
        }
        .info table { width: 100%; }
        .info td { padding: 6px 0; }
        .info td:first-child { font-weight: bold; width: 140px; }
        table.items { 
            width: 100%; 
            border-collapse: collapse; 
            margin: 20px 0;
            font-size: 11pt;
        }
        table.items th {
            background: #333;
            color: white;
            padding: 12px 8px;
            text-align: center;
            font-weight: bold;
        }
        table.items td {
            padding: 12px 8px;
            border-bottom: 1px solid #666;
            text-align: center;
        }
        table.items td:nth-child(2) { text-align: left; }
        .text-right { text-align: right; }
        .total {
            font-size: 16pt;
            font-weight: bold;
            color: #d32f2f;
            text-align: right;
            padding: 15px 10px;
            background: #f0f0f0;
            border-top: 3px double #333;
        }
        .footer {
            margin-top: 40px;
            text-align: center;
            font-size: 11pt;
            border-top: 1px dashed #666;
            padding-top: 15px;
        }
        .text-center { text-align: center; }
        .bold { font-weight: bold; }

        /* TỐI ƯU IN NHIỆT 80MM */
        @media print {
            body { padding: 3mm; font-size: 10pt; }
            .invoice { border: none; padding: 5px; }
            table.items th, table.items td { padding: 6px 4px; font-size: 9pt; }
            .header h1 { font-size: 18pt; }
            @page { size: 80mm auto; margin: 0; }
        }
    </style>
</head>
<body onload=""window.print()"">
<div class=""invoice"">
    <div class=""header"">
        <h1>FASHION STORE</h1>
        <p>123 Đường Thời Trang, Q.1, TP.HCM</p>
        <p>Hotline: 0909.123.456</p>
        <h2>HÓA ĐƠN BÁN HÀNG</h2>
        <p class=""bold"">Mã đơn: ").Append(maDonHang).Append(@" | ")
                  .Append(donHang.Ngay_Dat).Append(@"</p>
    </div>

    <div class=""info"">
        <table>
            <tr><td>Khách hàng:</td><td>").Append(donHang.KhachHang?.HoTen ?? "Khách lẻ").Append(@"</td></tr>
            <tr><td>Số điện thoại:</td><td>").Append(donHang.KhachHang?.SoDienThoai ?? "-").Append(@"</td></tr>
            <tr><td>Địa chỉ:</td><td>").Append(donHang.KhachHang?.DiaChi ?? "-").Append(@"</td></tr>
            <tr><td>Thanh toán:</td><td>").Append(donHang.PhuongThucThanhToan?.Ten_PhuongThuc ?? "COD").Append(@"</td></tr>
        </table>
    </div>

    <table class=""items"">
        <thead>
            <tr>
                <th width=""5%"">STT</th>
                <th width=""45%"">Sản phẩm</th>
                <th width=""20%"">Màu/Size</th>
                <th width=""10%"">SL</th>
                <th width=""10%"">Đơn giá</th>
                <th width=""10%"">Thành tiền</th>
            </tr>
        </thead>
        <tbody>");

            int stt = 1;
            foreach (var ct in donHang.ChiTietDonHangs!)
            {
                var thanhTien = ct.DonGia * ct.So_Luong;
                sb.Append($@"
            <tr>
                <td>{stt++}</td>
                <td style=""text-align:left"">
                    <div class=""bold"">{ct.SanPham?.Ten_SanPham}</div>
                    <small>Mã SP: {ct.Ma_SanPham}</small>
                </td>
                <td>{ct.BienThe?.Mau_Sac ?? "-"} / {ct.BienThe?.Kich_Thuoc ?? "-"}</td>
                <td>{ct.So_Luong}</td>
                <td class=""text-right"">{ct.DonGia:N0}</td>
                <td class=""text-right"">{thanhTien:N0}</td>
            </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>

    <div class=""total"">
        TỔNG TIỀN: {donHang.Tong_Tien:N0} ₫
    </div>

    <div class=""footer"">
        <p class=""bold"">Xin chân thành cảm ơn Quý khách!</p>
        <p>Đổi trả trong 7 ngày • Miễn phí vận chuyển đơn từ 500k</p>
    </div>
</div>
</body>
</html>");
            return sb.ToString();
        }

        // 1. LẤY ĐƠN HÀNG THEO TRẠNG THÁI + PHÂN QUYỀN (DÙNG CHO TẤT CẢ NHÂN VIÊN)
        public async Task<ResponseMessageResult> GetDonHangByTrangThaiAsync(string[] trangThaiChoPhep, string? search = null, int page = 1, int pageSize = 20)
        {
            var query = _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs!)
                    .ThenInclude(ct => ct.SanPham!)
                        .ThenInclude(sp => sp!.HinhAnhSanPhams!)
                .Where(d => trangThaiChoPhep.Contains(d.Trang_Thai));

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(d =>
                    d.Ma_DonHang.Contains(search) ||
                    d.KhachHang!.HoTen!.ToLower().Contains(search) ||
                    d.KhachHang!.SoDienThoai!.Contains(search));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(d => d.Ngay_Dat)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    d.Ma_DonHang,
                    d.Ngay_Dat,
                    KhachHang = d.KhachHang!.HoTen,
                    SoDienThoai = d.KhachHang.SoDienThoai,
                    d.Tong_Tien,
                    d.Trang_Thai,
                    SoSanPham = d.ChiTietDonHangs!.Count
                })
                .ToListAsync();

            return _response.SetSuccess("Lấy danh sách đơn hàng thành công!", new { Total = total, Items = items });
        }

        // 2. CẬP NHẬT TRẠNG THÁI CÓ KIỂM TRA QUYỀN + GHI LỊCH SỬ
        public async Task<ResponseMessageResult> CapNhatTrangThaiAsync(string maDonHang,string trangThaiMoi, string vaiTroNguoiThucHien,string tenNguoiThucHien)
        {
            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(d => d.Ma_DonHang == maDonHang);

            if (donHang == null)
                return _response.SetFail("Đơn hàng không tồn tại!", 404);

            // KIỂM TRA QUYỀN CHUYỂN TRẠNG THÁI
            bool choPhep = (vaiTroNguoiThucHien, donHang.Trang_Thai, trangThaiMoi) switch
            {
                ("Nhân viên bán hàng", "Chờ xác nhận", "Đang xác nhận") => true,
                ("Nhân viên bán hàng", "Đang xác nhận", "Chờ xử lý kho") => true,

                ("Nhân viên kho", "Chờ xử lý kho", "Đang đóng gói") => true,
                ("Nhân viên kho", "Đang đóng gói", "Chờ shipper") => true,

                ("Shipper", "Chờ shipper", "Đã lấy hàng") => true,
                ("Shipper", "Đã lấy hàng", "Đang giao") => true,
                ("Shipper", "Đang giao", "Đã giao") => true,
                ("Shipper", "Đang giao", "Giao không thành công") => true,

                _ => false
            };

            if (!choPhep)
                return _response.SetFail("Bạn không có quyền chuyển trạng thái này!", 403);

            var trangThaiCu = donHang.Trang_Thai;
            donHang.Trang_Thai = trangThaiMoi;
            //donHang.NgayCapNhat = DateTime.Now;

            //// GHI LỊCH SỬ
            //_context.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
            //{
            //    MaDonHang = maDonHang,
            //    TrangThaiCu = trangThaiCu,
            //    TrangThaiMoi = trangThaiMoi,
            //    NguoiThucHien = tenNguoiThucHien,
            //    VaiTro = vaiTroNguoiThucHien,
            //    ThoiGian = DateTime.Now
            //});

            await _context.SaveChangesAsync();
            return _response.SetSuccess("Cập nhật trạng thái thành công!", new { maDonHang, trangThaiMoi });
        }

        // 3. TRA CỨU KHÁCH HÀNG
        public async Task<ResponseMessageResult> TimKiemKhachHangAsync(string search)
        {
            var query = _context.KhachHangs
                .Where(k => k.SoDienThoai!.Contains(search) ||
                            k.HoTen!.ToLower().Contains(search.ToLower()));

            var result = await query.Select(k => new
            {
                k.Ma_KhachHang,
                k.HoTen,
                k.SoDienThoai,
                TongDonHang = _context.DonHangs.Count(d => d.Ma_KhachHang == k.Ma_KhachHang),
                TongTienMua = _context.DonHangs.Where(d => d.Ma_KhachHang == k.Ma_KhachHang).Sum(d => d.Tong_Tien)
            }).Take(20).ToListAsync();

            return _response.SetSuccess("Tìm khách hàng thành công!", result);
        }

        public async Task<ResponseMessageResult> GetDonHangByShipperAsync(int maShipper)
        {
            try
            {
                var donHangs = await _context.DonHangs
                    .Where(d => d.Ma_Shipper == maShipper && d.Trang_Thai== "Đang chờ shipper tới nhận hàng" ||d.Trang_Thai=="Đang giao" || d.Trang_Thai=="Đã giao")
                    .OrderByDescending(d => d.Ngay_Dat)
                      .Include(d => d.DiaChiGiaoHang)
                      .Include(pt=>pt.PhuongThucThanhToan)
                    .Select(d => new
                    {
                        d.Ma_DonHang,
                        d.Ngay_Dat,
                        d.Tong_Tien,
                        d.Trang_Thai,
                        d.PhuongThucThanhToan.Ten_PhuongThuc,
                        d.DiaChiGiaoHang,
                        ChiTiet = d.ChiTietDonHangs.Select(ct => new
                        {
                            ct.Ma_SanPham,
                            ct.Ma_BienThe,
                            ct.So_Luong,
                            ct.DonGia,
                            Ten_SanPham = ct.SanPham!.Ten_SanPham,
                            HinhAnh = ct.BienThe.HinhAnh
                                ?? "/images/no-image.jpg",
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

        // 4. LỊCH SỬ TRẠNG THÁI ĐƠN HÀNG
        //public async Task<ResponseMessageResult> GetLichSuTrangThaiAsync(string maDonHang)
        //{
        //    var lichSu = await _context.LichSuTrangThaiDonHangs
        //        .Where(l => l.MaDonHang == maDonHang)
        //        .OrderBy(l => l.ThoiGian)
        //        .Select(l => new
        //        {
        //            l.TrangThaiCu,
        //            l.TrangThaiMoi,
        //            l.NguoiThucHien,
        //            l.VaiTro,
        //            ThoiGian = l.ThoiGian.ToString("dd/MM/yyyy HH:mm")
        //        })
        //        .ToListAsync();

        //    return _response.SetSuccess("Lấy lịch sử thành công!", lichSu);
        //}
    }

    public class TaoDonHangRequest
    {
        public int Ma_KhachHang { get; set; }
        public int Ma_PhuongThuc { get; set; }
        public int Ma_DiaChi { get; set; }
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
