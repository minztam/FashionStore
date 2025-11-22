using Azure.Core;
using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models;
using FashionStore.Models.Momo;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services.Momo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly FashionStoreContext _context;
        private readonly IMomoService _momoService;
        private readonly IThanhToanRepository _thanhToanRepo;

        public ThanhToanController(FashionStoreContext context,
                                   IMomoService momoService,
                                   IThanhToanRepository thanhToanRepo)
        {
            _context = context;
            _momoService = momoService;
            _thanhToanRepo = thanhToanRepo;
        }

        
        [HttpPost("momo/thanhtoan")]
        public async Task<IActionResult> ThanhToanMomo(int maKhachHang, int maPhuongThuc)
        {
            if (maPhuongThuc <= 0)
                return BadRequest(new { message = "Vui lòng chọn phương thức thanh toán!" });

            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp!.BienThes)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return BadRequest(new { message = "Giỏ hàng rỗng!" });

            decimal tongTien = 0;
            var chiTietDonHangs = new List<ChiTietDonHang>();

            // TẠO ĐƠN HÀNG TRƯỚC – CHỈ TẠO 1 LẦN!
            var donHang = new DonHang
            {
                Ma_DonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Ma_KhachHang = maKhachHang,
                Tong_Tien = 0, // sẽ cập nhật sau
                Ma_PhuongThuc = maPhuongThuc,
                Trang_Thai = "Chờ thanh toán",
                Ngay_Dat = DateTime.Now
            };

            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync(); // Lưu để có Ma_DonHang

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                // Lấy biến thể (nếu có), không thì lấy giá sản phẩm gốc
                var bienThe = ct.SanPham?.BienThes?.FirstOrDefault();
                decimal gia = bienThe != null
                    ? (bienThe.Gia_Giam.HasValue && bienThe.Gia_Giam > 0 ? bienThe.Gia_Giam.Value : bienThe.Gia_BienThe) : 0;

                tongTien += gia * ct.So_Luong;

                chiTietDonHangs.Add(new ChiTietDonHang
                {
                    Ma_DonHang = donHang.Ma_DonHang,
                    Ma_SanPham = ct.Ma_SanPham,
                    So_Luong = ct.So_Luong,
                    DonGia = gia
                });
            }

            if (tongTien <= 0)
                return BadRequest(new { message = "Tổng tiền không hợp lệ!" });

            // CẬP NHẬT TỔNG TIỀN CHO ĐƠN HÀNG
            donHang.Tong_Tien = tongTien;
            _context.DonHangs.Update(donHang);

            // Thêm chi tiết đơn hàng
            _context.ChiTietDonHangs.AddRange(chiTietDonHangs);

            // XÓA GIỎ HÀNG
            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.GioHangs.Remove(gioHang);

            await _context.SaveChangesAsync();

            // GỌI MOMO
            var momoModel = new OrderInfoModel
            {
                OrderId = donHang.Ma_DonHang,
                Amount = tongTien,
                OrderInfo = $"Thanh toán đơn hàng #{donHang.Ma_DonHang} - FashionStore",
                FullName = "Khách hàng FashionStore"
            };

            var momoResult = await _momoService.CreatePaymentAsync(momoModel);

            if (string.IsNullOrEmpty(momoResult?.PayUrl))
                return BadRequest(new { message = "Tạo link MoMo thất bại!" });

            return Ok(new
            {
                success = true,
                message = "Tạo đơn hàng + link thanh toán MoMo thành công!",
                payUrl = momoResult.PayUrl,
                orderId = donHang.Ma_DonHang,
                tongTien = tongTien
            });
        }

        [HttpGet("MomoCallback")]
        public async Task<IActionResult> MomoCallback()
        {
            var momoRes = _momoService.PaymentExecute(Request.Query);

            if (!string.IsNullOrEmpty(momoRes.OrderId))
            {
                var donHang = await _context.DonHangs
                    .FirstOrDefaultAsync(d => d.Ma_DonHang == momoRes.OrderId);

                if (donHang != null && momoRes.ErrorCode == "0")  // “0” thành công
                {
                    donHang.Trang_Thai = "Đã thanh toán";
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Thanh toán thành công!", momoRes });
                }
            }

            return BadRequest(new { message = "Thanh toán thất bại hoặc đơn hàng không tìm thấy", momoRes });
        }

        [HttpPost("MomoNotify")]
        public async Task<IActionResult> MomoNotify([FromBody] MomoExecuteResponseModel momoRes)
        {
            if (momoRes != null && !string.IsNullOrEmpty(momoRes.OrderId) && momoRes.ErrorCode == "0")
            {
                var donHang = await _context.DonHangs
                    .FirstOrDefaultAsync(d => d.Ma_DonHang == momoRes.OrderId);

                if (donHang != null)
                {
                    donHang.Trang_Thai = "Đã thanh toán";
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Notify nhận OK", momoRes });
        }

        //======== _____ ========
        [HttpPost("thanh-toan-cod")]
        public async Task<IActionResult> ThanhToanCOD([FromBody] ThanhToanCODRequest request)
        {
            if (request == null || request.MaKhachHang <= 0)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!"));
            }
            var result = await _thanhToanRepo.ThanhToanCODAsync(request.MaKhachHang, request.MaVoucher);
            return StatusCode(result.StatusCode, result);
        }
    }

    // Request model
    public class ThanhToanCODRequest
    {
        public int MaKhachHang { get; set; }
        public string? MaVoucher { get; set; }
    }
}
