using Azure;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class BaoCaoRepository : IBaoCaoRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;
        public BaoCaoRepository(FashionStoreContext context, ResponseMessageResult response)
        {
            _context = context;
            _response = response;
        }

        public async Task<ResponseMessageResult> ThongKeDoanhThuAsync(DateTime? fromDate, DateTime? toDate, string? groupBy)
        {
            try
            {
                var query = _context.DonHangs.AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(d => d.Ngay_Dat >= fromDate.Value);
                if (toDate.HasValue)
                    query = query.Where(d => d.Ngay_Dat <= toDate.Value);

                List<ThongKeDoanhThuDTO> result = new();

                switch (groupBy?.ToLower())
                {
                    case "day":
                        result = await query
                            .GroupBy(d => new { d.Ngay_Dat.Year, d.Ngay_Dat.Month, d.Ngay_Dat.Day })
                            .Select(g => new ThongKeDoanhThuDTO
                            {
                                TimeLabel = $"{g.Key.Day}/{g.Key.Month}/{g.Key.Year}",
                                TongTien = g.Sum(x => x.Tong_Tien),
                                SoDonHang = g.Count()
                            })
                            .ToListAsync();
                        break;

                    case "month":
                        result = await query
                            .GroupBy(d => new { d.Ngay_Dat.Year, d.Ngay_Dat.Month })
                            .Select(g => new ThongKeDoanhThuDTO
                            {
                                TimeLabel = $"{g.Key.Month}/{g.Key.Year}",
                                TongTien = g.Sum(x => x.Tong_Tien),
                                SoDonHang = g.Count()
                            })
                            .ToListAsync();
                        break;

                    case "year":
                        result = await query
                            .GroupBy(d => d.Ngay_Dat.Year)
                            .Select(g => new ThongKeDoanhThuDTO
                            {
                                TimeLabel = g.Key.ToString(),
                                TongTien = g.Sum(x => x.Tong_Tien),
                                SoDonHang = g.Count()
                            })
                            .ToListAsync();
                        break;

                    default:
                        result = await query
                            .Select(d => new ThongKeDoanhThuDTO
                            {
                                TimeLabel = d.Ngay_Dat.ToString("dd/MM/yyyy"),
                                TongTien = d.Tong_Tien,
                                SoDonHang = 1
                            })
                            .ToListAsync();
                        break;
                }

                return _response.SetSuccess("Thống kê doanh thu thành công!", result);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi thống kê doanh thu: " + ex.Message, 500);
            }
        }
        public async Task<ResponseMessageResult> SanPhamBanChayAsync(int top = 10)
        {
            try
            {
                var result = await _context.ChiTietDonHangs
                    .Include(ct => ct.SanPham)
                    .GroupBy(ct => new { ct.Ma_SanPham, ct.SanPham!.Ten_SanPham })
                    .Select(g => new
                    {
                        g.Key.Ma_SanPham,
                        g.Key.Ten_SanPham,
                        SoLuongBan = g.Sum(x => x.So_Luong),
                        DoanhThu = g.Sum(x => x.So_Luong * x.DonGia)
                    })
                    .OrderByDescending(x => x.SoLuongBan)
                    .Take(top)
                    .ToListAsync();

                return _response.SetSuccess("Thống kê sản phẩm bán chạy thành công!", result);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi thống kê sản phẩm: " + ex.Message, 500);
            }
        }
        public async Task<ResponseMessageResult> KhachHangMoiAsync(DateTime? fromDate, DateTime? toDate, string? groupBy)
        {
            try
            {
                var query = _context.KhachHangs.AsQueryable();

                // --- lọc khoảng thời gian ---
                if (fromDate.HasValue)
                    query = query.Where(k => k.TaiKhoan.Ngay_Tao.HasValue && k.TaiKhoan.Ngay_Tao.Value >= fromDate.Value);
                if (toDate.HasValue)
                    query = query.Where(k => k.TaiKhoan.Ngay_Tao.HasValue && k.TaiKhoan.Ngay_Tao.Value <= toDate.Value);

                List<object> result = new();

                switch (groupBy?.ToLower())
                {
                    case "day":
                        result = await query
                            .GroupBy(k => new
                            {
                                Year = k.TaiKhoan.Ngay_Tao.Value.Year,
                                Month = k.TaiKhoan.Ngay_Tao.Value.Month,
                                Day = k.TaiKhoan.Ngay_Tao.Value.Day
                            })
                            .Select(g => new
                            {
                                Time = $"{g.Key.Day}/{g.Key.Month}/{g.Key.Year}",
                                SoLuong = g.Count()
                            })
                            .ToListAsync<object>();
                        break;

                    case "month":
                        result = await query
                            .GroupBy(k => new
                            {
                                Year = k.TaiKhoan.Ngay_Tao.Value.Year,
                                Month = k.TaiKhoan.Ngay_Tao.Value.Month
                            })
                            .Select(g => new
                            {
                                Time = $"{g.Key.Month}/{g.Key.Year}",
                                SoLuong = g.Count()
                            })
                            .ToListAsync<object>();
                        break;

                    case "year":
                        result = await query
                            .GroupBy(k => k.TaiKhoan.Ngay_Tao.Value.Year)
                            .Select(g => new
                            {
                                Time = g.Key.ToString(),
                                SoLuong = g.Count()
                            })
                            .ToListAsync<object>();
                        break;

                    default:
                        result = await query
                            .Where(k => k.TaiKhoan.Ngay_Tao.HasValue)
                            .Select(k => new
                            {
                                Time = k.TaiKhoan.Ngay_Tao.Value.ToString("dd/MM/yyyy"),
                                SoLuong = 1
                            })
                            .ToListAsync<object>();
                        break;
                }

                return _response.SetSuccess("Thống kê khách hàng mới thành công!", result);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi thống kê khách hàng: " + ex.Message, 500);
            }
        }



    }
}
