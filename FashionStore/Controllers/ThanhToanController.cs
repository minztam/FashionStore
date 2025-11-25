using FashionStore.Data;
using FashionStore.Models.VNPay;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services.Momo;
using FashionStore.Services.VnPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly FashionStoreContext _context;
        private readonly IMomoService _momoService;
        private readonly IThanhToanRepository _thanhToanRepo;
        private readonly IVnPayService _vnPayService;
        private readonly ResponseMessageResult _response;
        private readonly ILogger<ThanhToanController> _logger;

        public ThanhToanController(FashionStoreContext context,
                                   IMomoService momoService,
                                   IThanhToanRepository thanhToanRepo,
                                   IVnPayService vnPayService,
                                   ResponseMessageResult response,
                                   ILogger<ThanhToanController> logger)
        {
            _context = context;
            _momoService = momoService;
            _thanhToanRepo = thanhToanRepo;
            _vnPayService = vnPayService;
            _response = response;
            _logger = logger;
        }

        //======== thanh-toan-cod ========
        // POST : api/ThanhToan{ ThanhToanCODRequest }
        [HttpPost("thanh-toan-cod")]
        public async Task<IActionResult> ThanhToanCOD([FromBody] ThanhToanCODRequest request)
        {
            if (request == null || request.MaKhachHang <= 0)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!"));
            }
            var result = await _thanhToanRepo.ThanhToanCODAsync(request.MaKhachHang, request.MaDiaChi,request.MaVoucher);
            return StatusCode(result.StatusCode, result);
        }

        // POST : api/ThanhToan{ ThanhToanCODRequest }
        [HttpPost("thanh-toan-vnpay")]
        public async Task<IActionResult> TaoPaymentVNPAY([FromBody] ThanhToanCODRequest request)
        {
            if (request == null || request.MaKhachHang <= 0)
                return Ok(_response.SetFail("Dữ liệu không hợp lệ!", 400));

            var tinhTien = await TinhTienVaApVoucherAsync(request.MaKhachHang, request.MaVoucher);
            if (!tinhTien.Success) return Ok(tinhTien);

            var data = tinhTien.Data as dynamic;
            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            var paymentUrl = _vnPayService.CreatePaymentUrl(new PaymentInformationModel
            {
                OrderId = maDonHang,
                Amount = data!.TongThanhToan,
                OrderDescription = $"Thanh toan don hang {maDonHang} - Fashion Store",
                Name = "Fashion Store"
            }, HttpContext);

            // Lưu tạm mã đơn vào Session để callback dùng (tránh tạo trùng)
            HttpContext.Session.SetString("VNPAY_Pending_" + maDonHang, "true");

            return Ok(_response.SetSuccess("Chuyển đến VNPAY thành công!", new
            {
                MaDonHang = maDonHang,
                TongTien = data.TongThanhToan,
                PaymentUrl = paymentUrl
            }));
        }

        // VNPAY CALLBACK → XỬ LÝ THANH TOÁN + HOÀN TẤT ĐƠN
        [HttpGet("payment/callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var vnpayResult = _vnPayService.PaymentExecute(Request.Query);

            string? orderId = Request.Query["vnp_TxnRef"].FirstOrDefault();
            if (string.IsNullOrEmpty(orderId))
            {
                var orderInfo = Request.Query["vnp_OrderInfo"].FirstOrDefault() ?? "";
                orderId = Regex.Match(orderInfo, @"DH\d{14,}").Value;
            }

            string? maDiaChiStr = Request.Query["maDiaChi"].FirstOrDefault();
            if (string.IsNullOrEmpty(maDiaChiStr) || !int.TryParse(maDiaChiStr, out int maDiaChi))
            {
                return BadRequest("Mã địa chỉ không hợp lệ");
            }


            var response = new
            {
                Success = vnpayResult.Success,
                OrderId = orderId,
                ResponseCode = vnpayResult.Success ? "00" : "99",
                Message = vnpayResult.Success ? "Thanh toán thành công!" : vnpayResult.Message ?? "Thanh toán thất bại!"
            };

            if (vnpayResult.Success && !string.IsNullOrEmpty(orderId))
            {
                var daTonTai = await _context.DonHangs.AnyAsync(d => d.Ma_DonHang == orderId);
                if (!daTonTai)
                {
                    // Lấy MaKhachHang từ giỏ hàng còn tồn tại
                    var gioHang = await _context.GioHangs
                        .FirstOrDefaultAsync(g => g.ChiTietGioHangs.Any(ct => ct.So_Luong > 0));

                    if (gioHang != null)
                    {
                        var result = await _thanhToanRepo.TaoDonHangKhiVNPAYThanhCongAsync(
                            orderId,
                            gioHang.Ma_KhachHang,
                            maDiaChi // dùng Ma_DiaChi từ frontend
                        );

                        if (!result.Success)
                        {
                            _logger.LogError("Tạo đơn thất bại sau VNPAY thành công: {OrderId} - {Error}", orderId, result.Message);
                        }
                    }
                }
            }

            return Ok(response);
        }

        // HÀM TÍNH TIỀN + ÁP VOUCHER (DÙNG CHUNG CHO COD & VNPAY)
        private async Task<ResponseMessageResult> TinhTienVaApVoucherAsync(int maKhachHang, string? maVoucher)
        {
            try
            {
                var gioHang = await _context.GioHangs
                    .Include(g => g.ChiTietGioHangs!)
                        .ThenInclude(ct => ct.BienThe!)
                            .ThenInclude(bt => bt!.SanPham!)
                    .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any(x => x.So_Luong > 0))
                    return _response.SetFail("Giỏ hàng trống!", 400);

                var chiTiet = gioHang.ChiTietGioHangs.Where(x => x.So_Luong > 0).ToList();

                decimal tongGoc = 0;
                decimal giamGia = 0;

                foreach (var ct in chiTiet)
                {
                    var gia = ct.BienThe!.Gia_Giam ?? ct.BienThe.Gia_BienThe;
                    tongGoc += gia * ct.So_Luong;
                }

                // Áp voucher (nếu có)
                if (!string.IsNullOrEmpty(maVoucher))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher && v.So_LanDung > 0 && v.Ngay_KetThuc >= DateTime.Now);

                    if (voucher != null)
                    {
                        giamGia = (decimal)voucher.GiaTri_ToiThieu!;
                        voucher.So_LanDung -= 1; // giảm số lượng voucher
                    }
                }

                decimal tongThanhToan = tongGoc - giamGia;
                if (tongThanhToan < 0) tongThanhToan = 0;

                return _response.SetSuccess("Tính tiền thành công!", new
                {
                    TongGoc = tongGoc,
                    GiamGia = giamGia,
                    TongThanhToan = tongThanhToan
                });
            }
            catch (Exception ex)
            {
                return _response.SetFail("Lỗi tính tiền: " + ex.Message, 500);
            }
        }
    }

    // Request model
    public class ThanhToanCODRequest
    {
    
        public int MaKhachHang { get; set; }
        public string? MaVoucher { get; set; }
         public int MaDiaChi { get; set; }
    }
}
