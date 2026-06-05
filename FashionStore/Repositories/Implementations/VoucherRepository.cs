using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FashionStore.Repositories.Implementations
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly FashionStoreContext _context;
        private readonly ResponseMessageResult _response;
        public VoucherRepository(FashionStoreContext context, ResponseMessageResult response)
        {
            _context = context;
            _response = response;
        }
        public async Task<ResponseMessageResult> GetAllAsync()
        {
            try
            {
                var vouchers = await _context.Vouchers
                    .AsNoTracking() // Tối ưu performance
                    .OrderByDescending(v => v.Ngay_BatDau)
                    .ToListAsync();

                var dtos = vouchers.Select(v => new VoucherDTO
                {
                    Ma_Voucher = v.Ma_Voucher,
                    Giam_PhanTram = v.Giam_PhanTram,
                    Giam_Tien = v.Giam_Tien,
                    GiaTri_ToiThieu = v.GiaTri_ToiThieu,
                    So_LanDung = v.So_LanDung,
                    Ngay_BatDau = v.Ngay_BatDau,
                    Ngay_KetThuc = v.Ngay_KetThuc,
                    Trang_Thai = v.Trang_Thai
                }).ToList();

                return _response.SetSuccess("Lấy danh sách voucher thành công!", dtos);
            }
            catch (Exception ex)
            {
                // Ghi log ở đây nếu có ILogger
                //_logger.LogError(ex, "Lỗi khi lấy danh sách voucher");
                return _response.SetFail("Lỗi hệ thống khi lấy danh sách voucher!" + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> GetByCodeAsync(string maVoucher)
        {
            try
            {
                var voucher = await _context.Vouchers
                    .Include(v => v.DonHangs) // Để đếm lượt dùng
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Ma_Voucher.ToUpper() == maVoucher.Trim().ToUpper());

                if (voucher == null)
                    return _response.SetFail("Không tìm thấy voucher!", 404);

                var dto = new VoucherDTO
                {
                    Ma_Voucher = voucher.Ma_Voucher,
                    Giam_PhanTram = voucher.Giam_PhanTram,
                    Giam_Tien = voucher.Giam_Tien,
                    GiaTri_ToiThieu = voucher.GiaTri_ToiThieu,
                    So_LanDung = voucher.So_LanDung,
                    Ngay_BatDau = voucher.Ngay_BatDau,
                    Ngay_KetThuc = voucher.Ngay_KetThuc,
                    Trang_Thai = voucher.Trang_Thai
                };

                return _response.SetSuccess("Lấy danh sách voucher thành công!", dto);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi hệ thống khi lấy voucher: " + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> AddAsync(VoucherDTO voucherDto)
        {
            try
            {
                // Kiểm tra trùng mã
                bool isExist = await _context.Vouchers
                    .AnyAsync(v => v.Ma_Voucher.ToUpper() == voucherDto.Ma_Voucher.Trim().ToUpper());

                if (isExist)
                    return _response.SetFail("Mã voucher đã tồn tại!");

                // CHUYỂN DTO → ENTITY (QUAN TRỌNG NHẤT!!!)
                var voucher = new Voucher
                {
                    Ma_Voucher = voucherDto.Ma_Voucher.Trim().ToUpper(),
                    Giam_PhanTram = voucherDto.Giam_PhanTram,
                    Giam_Tien = voucherDto.Giam_Tien,
                    GiaTri_ToiThieu = voucherDto.GiaTri_ToiThieu,
                    So_LanDung = voucherDto.So_LanDung,
                    Ngay_BatDau = voucherDto.Ngay_BatDau,
                    Ngay_KetThuc = voucherDto.Ngay_KetThuc,
                    Trang_Thai = voucherDto.Trang_Thai == true
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();

                return _response.SetSuccess("Tạo voucher thành công!", voucherDto);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Tạo voucher thất bại: " + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> UpdateAsync(VoucherDTO voucherDto)
        {
            try
            {
                var existingVoucher = await _context.Vouchers
                    .Include(v => v.DonHangs)
                    .FirstOrDefaultAsync(v => v.Ma_Voucher.ToUpper() == voucherDto.Ma_Voucher.Trim().ToUpper());
                if (existingVoucher == null)
                {
                    return _response.SetFail("Voucher không tồn tại", 404);
                }

                existingVoucher.Giam_PhanTram = voucherDto.Giam_PhanTram;
                existingVoucher.Giam_Tien = voucherDto.Giam_Tien;
                existingVoucher.GiaTri_ToiThieu = voucherDto.GiaTri_ToiThieu;
                existingVoucher.So_LanDung = voucherDto.So_LanDung;
                existingVoucher.Ngay_BatDau = voucherDto.Ngay_BatDau;
                existingVoucher.Ngay_KetThuc = voucherDto.Ngay_KetThuc;
                existingVoucher.Trang_Thai = voucherDto.Trang_Thai.HasValue;

                await _context.SaveChangesAsync();
                return _response.SetSuccess("Cập nhật voucher thành công", voucherDto);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Cập nhật voucher thất bại: " + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> PatchAsync(string maVoucher, VoucherDTO patchDto)
        {
            try
            {
                var voucher = await _context.Vouchers
                    .Include(v => v.DonHangs)
                    .FirstOrDefaultAsync(v => v.Ma_Voucher.ToUpper() == maVoucher.Trim().ToUpper());

                if (voucher == null)
                    return _response.SetFail("Voucher không tồn tại!", 404);

                // CHỈ CẬP NHẬT NHỮNG TRƯỜNG CÓ GIÁ TRỊ (khác null)
                if (patchDto.Giam_PhanTram != null)
                    voucher.Giam_PhanTram = patchDto.Giam_PhanTram;

                if (patchDto.Giam_Tien != null)
                    voucher.Giam_Tien = patchDto.Giam_Tien;

                if (patchDto.GiaTri_ToiThieu != null)
                    voucher.GiaTri_ToiThieu = patchDto.GiaTri_ToiThieu;

                if (patchDto.So_LanDung != null)
                    voucher.So_LanDung = patchDto.So_LanDung;

                if (patchDto.Ngay_BatDau != null)
                    voucher.Ngay_BatDau = patchDto.Ngay_BatDau;

                if (patchDto.Ngay_KetThuc != null)
                    voucher.Ngay_KetThuc = patchDto.Ngay_KetThuc;

                if (patchDto.Trang_Thai != null)
                    voucher.Trang_Thai = patchDto.Trang_Thai.Value;

                await _context.SaveChangesAsync();

                // Trả về DTO đầy đủ + thông tin tính toán
                var resultDto = new VoucherDTO
                {
                    Ma_Voucher = voucher.Ma_Voucher,
                    Giam_PhanTram = voucher.Giam_PhanTram,
                    Giam_Tien = voucher.Giam_Tien,
                    GiaTri_ToiThieu = voucher.GiaTri_ToiThieu,
                    So_LanDung = voucher.So_LanDung,
                    Ngay_BatDau = voucher.Ngay_BatDau,
                    Ngay_KetThuc = voucher.Ngay_KetThuc,
                    Trang_Thai = voucher.Trang_Thai
                };

                return _response.SetSuccess("Cập nhật voucher thành công!", resultDto);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Cập nhật thất bại: " + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> DeleteAsync(string maVoucher)
        {
            try
            {
                // Tìm voucher (không phân biệt hoa thường + trim)
                var voucher = await _context.Vouchers
                    .FirstOrDefaultAsync(v => v.Ma_Voucher.ToUpper() == maVoucher.Trim().ToUpper());

                if (voucher == null)
                    return _response.SetFail("Không tìm thấy voucher với mã: " + maVoucher, 404);

                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();

                return _response.SetSuccess("Xóa voucher thành công!");
            }
            catch (Exception ex)
            {
                return _response.SetFail("Xóa voucher thất bại: " + ex.Message, 500);
            }
        }

        public async Task<ResponseMessageResult> KiemTraVaTinhGiamGiaAsync(string maVoucher, decimal tongTien)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maVoucher))
                    return _response.SetFail("Mã voucher không được để trống!", 400);

                if (tongTien <= 0)
                    return _response.SetFail("Tổng tiền phải lớn hơn 0!", 400);

                var voucher = await _context.Vouchers
                    .Include(v => v.DonHangs)
                    .FirstOrDefaultAsync(v => v.Ma_Voucher.ToUpper() == maVoucher.Trim().ToUpper());

                if (voucher == null)
                    return _response.SetFail("Voucher không tồn tại!", 404);

                if (!voucher.Trang_Thai)
                    return _response.SetFail("Voucher đã bị vô hiệu hóa!", 400);

                if (voucher.Ngay_BatDau.HasValue && voucher.Ngay_BatDau.Value.Date > DateTime.Now.Date)
                    return _response.SetFail("Voucher chưa đến thời gian sử dụng!", 400);

                if (voucher.Ngay_KetThuc.HasValue && voucher.Ngay_KetThuc.Value.Date < DateTime.Now.Date)
                    return _response.SetFail("Voucher đã hết hạn!", 400);

                if (voucher.GiaTri_ToiThieu.HasValue && tongTien < voucher.GiaTri_ToiThieu.Value)
                    return _response.SetFail($"Đơn hàng phải từ {voucher.GiaTri_ToiThieu:#,##0}đ trở lên!", 400);

                int daDung = voucher.DonHangs?.Count ?? 0;
                if (voucher.So_LanDung.HasValue && daDung >= voucher.So_LanDung.Value)
                    return _response.SetFail("Voucher đã hết lượt sử dụng!", 400);

                // TÍNH GIẢM GIÁ CHÍNH XÁC NHẤT
                decimal giamGia = 0;

                if (voucher.Giam_PhanTram.HasValue && voucher.Giam_PhanTram.Value > 0)
                {
                    giamGia = tongTien * voucher.Giam_PhanTram.Value / 100;
                }

                if (voucher.Giam_Tien.HasValue && voucher.Giam_Tien.Value > 0)
                {
                    // ƯU TIÊN GIẢM TIỀN NẾU CÓ, HOẶC LẤY GIÁ TRỊ LỚN HƠN
                    giamGia = Math.Max(giamGia, voucher.Giam_Tien.Value);
                }

                // KHÔNG BAO GIỜ ĐƯỢC GIẢM QUÁ TỔNG TIỀN
                giamGia = Math.Min(giamGia, tongTien);

                var result = new
                {
                    Ma_Voucher = voucher.Ma_Voucher,
                    Loai_Giam = voucher.Giam_PhanTram.HasValue ? "Phần trăm" : "Tiền",
                    Giam_Gia = giamGia,
                    Tong_Tien_Sau_Giam = tongTien - giamGia,
                    Con_Lai = voucher.So_LanDung.HasValue ? voucher.So_LanDung.Value - daDung : (int?)null,
                    Het_Han = voucher.Ngay_KetThuc
                };

                return _response.SetSuccess("Áp dụng voucher thành công!", result);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi hệ thống: " + ex.Message, 500);
            }
        }

        // Áp dụng voucher (giảm số lần dùng, giả định đã kiểm tra trước)
        public async Task<ResponseMessageResult> ApDungVoucherAsync(string maVoucher)
        {
            var voucher = await _context.Vouchers.FindAsync(maVoucher);
            if (voucher == null || !voucher.Trang_Thai)
            {
                return new ResponseMessageResult().SetFail("Voucher không tồn tại hoặc không hoạt động");
            }
            if (voucher.So_LanDung.HasValue && voucher.So_LanDung.Value <= 0)
            {
                return new ResponseMessageResult().SetFail("Voucher đã hết lượt sử dụng");
            }
            // Giảm số lần dùng
            if (voucher.So_LanDung.HasValue)
            {
                voucher.So_LanDung -= 1;
            }
            await _context.SaveChangesAsync();
            return new ResponseMessageResult().SetSuccess("Áp dụng voucher thành công", voucher);
        }

        public async Task<ResponseMessageResult> ApplyVoucherGioHangAsync(int maKhachHang, string maVoucher)
        {
            // Lấy giỏ hàng
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp!.BienThes)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return new ResponseMessageResult().SetFail("Giỏ hàng rỗng");

            // Lấy voucher
            var voucher = await _context.Vouchers.FindAsync(maVoucher);

            if (voucher == null)
                return new ResponseMessageResult().SetFail("Voucher không tồn tại");

            // Kiểm tra trạng thái
            if (!voucher.Trang_Thai)
                return new ResponseMessageResult().SetFail("Voucher không còn hoạt động");

            DateTime now = DateTime.Now;

            if (voucher.Ngay_BatDau > now || voucher.Ngay_KetThuc < now)
                return new ResponseMessageResult().SetFail("Voucher đã hết hạn hoặc chưa đến thời gian sử dụng");

            if (voucher.So_LanDung <= 0)
                return new ResponseMessageResult().SetFail("Voucher đã hết lượt dùng");

            // Tính tổng tiền giỏ hàng
            decimal tongTien = 0;

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                var bienThe = ct.SanPham?.BienThes.FirstOrDefault();
                decimal gia = (bienThe?.Gia_Giam > 0) ? bienThe.Gia_Giam.Value : bienThe?.Gia_BienThe ?? 0;
                tongTien += gia * ct.So_Luong;
            }

            if (voucher.GiaTri_ToiThieu.HasValue && tongTien < voucher.GiaTri_ToiThieu.Value)
                return new ResponseMessageResult().SetFail("Không đủ giá trị tối thiểu để áp dụng voucher");

            // Tính giảm giá
            decimal giam = 0;

            if (voucher.Giam_PhanTram.HasValue)
                giam += tongTien * voucher.Giam_PhanTram.Value / 100;

            if (voucher.Giam_Tien.HasValue)
                giam += voucher.Giam_Tien.Value;

            decimal tongSauGiam = tongTien - giam;

            // Trả dữ liệu
            return new ResponseMessageResult().SetSuccess("Áp dụng voucher thành công", new
            {
                TongGoc = tongTien,
                GiamGia = giam,
                TongSauGiam = tongSauGiam
            });
        }

      
    }
}
