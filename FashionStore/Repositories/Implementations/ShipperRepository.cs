using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class ShipperRepository(
        FashionStoreContext _context,
        ResponseMessageResult _response
    ) : IShipperRepository
    {
        // ----------------------------------
        // GET ALL
        // ----------------------------------
        public async Task<ResponseMessageResult> GetAllAsync()
        {
            try
            {
                var rs = await _context.Shippers
                    .Include(s => s.TaiKhoan)
                    .Select(s => new
                    {
                        s.Ma_Shipper,
                        s.Ma_TaiKhoan,
                        s.Ten_DayDu,
                        s.SoDienThoai,
                        s.BienSoXe,
                        s.TrangThai
                    })
                    .ToListAsync();

                if (rs.Count == 0)
                    return _response.SetCustom(true, null!, 200, null);

                return _response.SetSuccess("Lấy danh sách shipper thành công", rs);
            }
            catch
            {
                return _response.SetFail("Có lỗi khi lấy danh sách shipper", 500);
            }
        }

        // ----------------------------------
        // GET BY ID
        // ----------------------------------
        public async Task<ResponseMessageResult> GetByIdAsync(int maShipper)
        {
            try
            {
                var shipper = await _context.Shippers
                    .Where(s => s.Ma_Shipper == maShipper)
                    .Select(s => new
                    {
                        s.Ma_Shipper,
                        s.Ma_TaiKhoan,
                        s.Ten_DayDu,
                        s.SoDienThoai,
                        s.BienSoXe,
                        s.TrangThai
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (shipper == null)
                    return _response.SetFail("Không tìm thấy shipper", 404);

                return _response.SetSuccess("Lấy thông tin shipper thành công", shipper);
            }
            catch
            {
                return _response.SetFail("Có lỗi khi lấy shipper", 500);
            }
        }

        // ----------------------------------
        // ADD
        // ----------------------------------
        public async Task<ResponseMessageResult> AddAsync(ShipperRequestDTO dto)
        {
            if (dto == null)
                return _response.SetFail("Dữ liệu không hợp lệ", 400);

            // ✅ CHECK FK: Tài khoản
            var taiKhoanExists = await _context.TaiKhoans
                .AnyAsync(t => t.Ma_TaiKhoan == dto.Ma_TaiKhoan);

            if (!taiKhoanExists)
                return _response.SetFail("Tài khoản không tồn tại", 400);

            // ✅ CHECK: 1 tài khoản chỉ là 1 shipper
            var shipperExists = await _context.Shippers
                .AnyAsync(s => s.Ma_TaiKhoan == dto.Ma_TaiKhoan);

            if (shipperExists)
                return _response.SetFail("Tài khoản này đã là shipper", 400);

            try
            {
                var shipper = new Shipper
                {
                    Ma_TaiKhoan = dto.Ma_TaiKhoan,
                    Ten_DayDu = dto.Ten_DayDu,
                    SoDienThoai = dto.SoDienThoai,
                    BienSoXe = dto.BienSoXe,
                    TrangThai = "offline"
                };

                await _context.Shippers.AddAsync(shipper);
                await _context.SaveChangesAsync();

                return _response.SetSuccess("Tạo shipper thành công", shipper);
            }
            catch (Exception ex)
            {
                return _response.SetFail("Có lỗi khi tạo shipper", 500);
            }
        }


        // ----------------------------------
        // UPDATE
        // ----------------------------------
        public async Task<ResponseMessageResult> UpdateAsync(int maShipper, ShipperRequestDTO shipper)
        {
            try
            {
                var existing = await _context.Shippers
                    .FirstOrDefaultAsync(s => s.Ma_Shipper == maShipper);

                if (existing == null)
                    return _response.SetFail("Không tìm thấy shipper", 404);

                existing.Ten_DayDu = shipper.Ten_DayDu;
                existing.SoDienThoai = shipper.SoDienThoai;
                existing.BienSoXe = shipper.BienSoXe;

                await _context.SaveChangesAsync();

                return _response.SetSuccess("Cập nhật shipper thành công", existing);
            }
            catch
            {
                return _response.SetFail("Có lỗi khi cập nhật shipper", 500);
            }
        }

        // ----------------------------------
        // DELETE
        // ----------------------------------
        public async Task<ResponseMessageResult> DeleteAsync(int maShipper)
        {
            try
            {
                var shipper = await _context.Shippers
                    .FirstOrDefaultAsync(s => s.Ma_Shipper == maShipper);

                if (shipper == null)
                    return _response.SetFail("Không tìm thấy shipper", 404);

                _context.Shippers.Remove(shipper);
                await _context.SaveChangesAsync();

                return _response.SetSuccess("Xóa shipper thành công");
            }
            catch
            {
                return _response.SetFail("Không thể xóa shipper (đang được gán đơn?)", 500);
            }
        }

        // ----------------------------------
        // TOGGLE STATUS
        // ----------------------------------
        public async Task<ResponseMessageResult> ToggleStatusAsync(int maShipper)
        {
            try
            {
                var shipper = await _context.Shippers.FindAsync(maShipper);
                if (shipper == null)
                    return _response.SetFail("Không tìm thấy shipper", 404);

                // Toggle trạng thái
                shipper.TrangThai = shipper.TrangThai == "online" ? "offline" : "online";
                await _context.SaveChangesAsync();

                string message = shipper.TrangThai == "online" ? "Kích hoạt shipper" : "Vô hiệu hóa shipper";
                return _response.SetSuccess(message);
            }
            catch
            {
                return _response.SetFail("Có lỗi khi đổi trạng thái shipper", 500);
            }
        }

    }
}

