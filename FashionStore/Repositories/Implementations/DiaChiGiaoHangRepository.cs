using Azure;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Repositories.Implementations
{
    public class DiaChiGiaoHangRepository (FashionStoreContext _context, ResponseMessageResult _response) : IDiaChiGiaoHangRepository
    {
       

        public async Task<ResponseMessageResult> GetAddress(int maKhachHang)
        {
            var address =await _context.DiaChiGiaoHangs.Where(m=>m.Ma_KhachHang==maKhachHang && m.IsActive==true).ToListAsync();
            if (address.Count == 0)
            {
                _response.SetCustom(true, null!, 200, null);
            }
            _response.SetSuccess("Lấy danh sách danh mục thành công", address);
            return _response;
           
           
        }
        // 2. Thêm địa chỉ
        public async Task<ResponseMessageResult> AddAddress(DiaChiGiaoHangDTO model)
        {
            var entity = new DiaChiGiaoHang
            {
                HoTen_NguoiNhan = model.TenNguoiNhan,
                SoDienThoai = model.SDT,
                DiaChi_ChiTiet = model.Dia_Chi,
                GhiChu = model.Ghi_Chu,
                Ma_KhachHang = model.Ma_KhachHang
            };
            await _context.DiaChiGiaoHangs.AddAsync(entity);
            await _context.SaveChangesAsync();

            _response.SetSuccess("Thêm địa chỉ giao hàng thành công", model);
            return _response;
        }

        // 3. Sửa địa chỉ
        public async Task<ResponseMessageResult> UpdateAddress(int id, DiaChiGiaoHangDTO model)
        {
            var address = await _context.DiaChiGiaoHangs.FindAsync(id);
            if (address == null)
            {
                _response.SetCustom(false, "Không tìm thấy địa chỉ", 404, null);
                return _response;
            }

            // kiểm tra xem địa chỉ cũ có từng dùng cho đơn hàng nào chưa
            bool isUsedByOrder = await _context.DonHangs.AnyAsync(o => o.Ma_DiaChi == id);

            // nếu đã dùng rồi -> clone địa chỉ mới
            if (isUsedByOrder)
            {
                var newAddress = new DiaChiGiaoHang
                {
                    HoTen_NguoiNhan = model.TenNguoiNhan,
                    SoDienThoai = model.SDT,
                    DiaChi_ChiTiet = model.Dia_Chi,
                    GhiChu = model.Ghi_Chu,
                    Ma_KhachHang = model.Ma_KhachHang,
                    IsActive = true
                };

                // địa chỉ cũ không xóa — chỉ ẩn
                address.IsActive = false;

                _context.DiaChiGiaoHangs.Update(address);
                await _context.DiaChiGiaoHangs.AddAsync(newAddress);
                await _context.SaveChangesAsync();

                _response.SetSuccess("Địa chỉ đã dùng cho đơn hàng — tạo bản ghi mới thành công", newAddress);
                return _response;
            }
            else
            {
                // chưa từng được dùng → update bình thường
                address.HoTen_NguoiNhan = model.TenNguoiNhan;
                address.SoDienThoai = model.SDT;
                address.DiaChi_ChiTiet = model.Dia_Chi;
                address.GhiChu = model.Ghi_Chu;
                address.Ma_KhachHang = model.Ma_KhachHang;

                _context.DiaChiGiaoHangs.Update(address);
                await _context.SaveChangesAsync();

                _response.SetSuccess("Cập nhật địa chỉ thành công", address);
                return _response;
            }
        }


        // 4. Xóa địa chỉ
        public async Task<ResponseMessageResult> DeleteAddress(int maDiaChi)
        {
            var address = await _context.DiaChiGiaoHangs.FirstOrDefaultAsync(c=>c.Ma_DiaChi==maDiaChi);
            if (address == null)
            {
                _response.SetCustom(false, "Không tìm thấy địa chỉ để xóa", 404, null);
                return _response;
            }
              address.IsActive = false;
            _context.DiaChiGiaoHangs.Remove(address);
            await _context.SaveChangesAsync();

            _response.SetSuccess("Xóa địa chỉ giao hàng thành công", null);
            return _response;
        }
    }
}
