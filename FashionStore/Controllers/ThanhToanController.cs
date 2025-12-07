using FashionStore.Data;
using FashionStore.DTO;
using FashionStore.Models.VNPay;
using FashionStore.Repositories.Interfaces;
using FashionStore.Repositories.ResponseMessage;
using FashionStore.Services.Momo;
using FashionStore.Services.VnPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
        private readonly IConfiguration _config;


        public ThanhToanController(FashionStoreContext context,
                                   IMomoService momoService,
                                   IThanhToanRepository thanhToanRepo,
                                   IVnPayService vnPayService,
                                   ResponseMessageResult response,
                                   ILogger<ThanhToanController> logger,
                                   IConfiguration config)
        {
            _context = context;
            _momoService = momoService;
            _thanhToanRepo = thanhToanRepo;
            _vnPayService = vnPayService;
            _response = response;
            _logger = logger;
            _config = config;
        }

        //======== thanh-toan-cod ========
        // POST : api/ThanhToan{ ThanhToanCODRequest }
        [HttpPost("thanh-toan-cod")]
        public async Task<IActionResult> ThanhToanCOD([FromBody] ThanhToanCODRequest request)
        {
            if (request == null || request.Ma_KhachHang <= 0)
            {
                return BadRequest(new ResponseMessageResult().SetFail("Dữ liệu không hợp lệ!"));
            }
            var result = await _thanhToanRepo.ThanhToanCODAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        // POST : api/ThanhToan{ ThanhToanCODRequest }
        [HttpPost("thanh-toan-vnpay")]
        public async Task<IActionResult> TaoPaymentVNPAY([FromBody] ThanhToanCODRequest request)
        {
            if (request == null || request.Ma_KhachHang <= 0 || request.ChiTiet == null || !request.ChiTiet.Any())
                return Ok(_response.SetFail("Dữ liệu không hợp lệ!", 400));

            var tinhTien = await TinhTienVaApVoucherPartialAsync(request.Ma_KhachHang, request.ChiTiet, request.Ma_Voucher);
            if (!tinhTien.Success) return Ok(tinhTien);
            var data = tinhTien.Data as dynamic;

            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            var paymentUrl = _vnPayService.CreatePaymentUrl(new PaymentInformationModel
            {
                OrderId = maDonHang,
                Amount = data!.TongThanhToan,
                OrderDescription = $"Thanh toán đơn hàng {maDonHang} - Fashion Store",
                Name = "Fashion Store"
            }, HttpContext);

            await Sel(request.ChiTiet);

            return Ok(_response.SetSuccess("Chuyển đến VNPAY thành công!", new
            {
                MaDonHang = maDonHang,
                TongTien = data.TongThanhToan,
                PaymentUrl = paymentUrl
            }));
        }


        [HttpGet("payment/callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var vnpayResult = _vnPayService.PaymentExecute(Request.Query);
            var responseCode = Request.Query["vnp_ResponseCode"].FirstOrDefault();

            var orderId = Request.Query["vnp_TxnRef"].FirstOrDefault();
            if (string.IsNullOrEmpty(orderId))
            {
                var orderInfo = Request.Query["vnp_OrderInfo"].FirstOrDefault() ?? "";
                orderId = Regex.Match(orderInfo, @"DH\\d{14,}").Value;
            }

            if (string.IsNullOrEmpty(orderId))
                return Redirect($"{_config["FrontendUrl"]}/order-failure?msg=KhongTimThayMaDon");

            if (!vnpayResult.Success || responseCode != "00")
                return Redirect($"{_config["FrontendUrl"]}/order-failure?code={responseCode}");

            if (await _context.DonHangs.AnyAsync(d => d.Ma_DonHang == orderId))
                return Redirect($"{_config["FrontendUrl"]}/order-success?orderId={orderId}");

            // LẤY GIỎ HÀNG CỦA KHÁCH – CHỈ LẤY NHỮNG SẢN PHẨM CÓ So_Luong > 0 (ĐÃ ĐƯỢC CHỌN)
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs!)
                    .ThenInclude(ct => ct.BienThe!)
                        .ThenInclude(bt => bt!.SanPham!)
                .FirstOrDefaultAsync(g => g.ChiTietGioHangs.Any(ct => ct.So_Luong > 0));

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any(ct => ct.So_Luong > 0))
                return Redirect($"{_config["FrontendUrl"]}/order-failure?msg=GioHangTrong");

            int maKhachHang = gioHang.Ma_KhachHang;

            var diaChi = await _context.DiaChiGiaoHangs
                .FirstOrDefaultAsync(d => d.Ma_KhachHang == maKhachHang && d.IsActive);

            if (diaChi == null)
                return Redirect($"{_config["FrontendUrl"]}/order-failure?msg=KhongCoDiaChi");

            // GỌI HÀM HOÀN HẢO CỦA BẠN – NÓ SẼ CHỈ TẠO ĐƠN CHO SẢN PHẨM CÓ So_Luong > 0
            var result = await _thanhToanRepo.TaoDonHangKhiVNPAYThanhCongAsync(
                orderId,
                maKhachHang,
                diaChi.Ma_DiaChi
            );

            return result.Success
                ? Redirect($"{_config["FrontendUrl"]}/order-success?orderId={orderId}")
                : Redirect($"{_config["FrontendUrl"]}/order-failure?msg=TaoDonThatBai");
        }
        // HÀM TÍNH TIỀN + ÁP VOUCHER (DÙNG CHUNG CHO COD & VNPAY)
        private async Task<ResponseMessageResult> TinhTienVaApVoucherPartialAsync(
       int maKhachHang,
       List<CheckoutItem> chiTiet,
       string? maVoucher = null)
        {
            // Lấy giỏ hàng khách
            var gioHang = await _context.GioHangs
                .Include(g => g.ChiTietGioHangs!)
                    .ThenInclude(ct => ct.BienThe!)
                .FirstOrDefaultAsync(g => g.Ma_KhachHang == maKhachHang);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return _response.SetFail("Giỏ hàng trống!", 400);

            var selectedItems = gioHang.ChiTietGioHangs
                .Where(x => chiTiet.Select(c => c.Ma_BienThe).Contains(x.Ma_BienThe))
                .ToList();

            if (!selectedItems.Any())
                return _response.SetFail("Không có sản phẩm hợp lệ để thanh toán!", 400);

            decimal tongTienGoc = 0;

            foreach (var item in selectedItems)
            {
                var sl = chiTiet.First(c => c.Ma_BienThe == item.Ma_BienThe).So_Luong;

                if (item.BienThe == null || item.BienThe.So_Luong < sl)
                    return _response.SetFail($"Sản phẩm {item.Ma_SanPham} không đủ số lượng!", 400);

                decimal giaBan = item.BienThe.Gia_Giam ?? item.BienThe.Gia_BienThe;
                tongTienGoc += giaBan * sl;
            }

            // Áp voucher (nếu có)
            decimal giamGia = 0;
            if (!string.IsNullOrEmpty(maVoucher))
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Ma_Voucher == maVoucher && v.Trang_Thai && v.So_LanDung > 0);
                if (voucher != null)
                {
                    giamGia = voucher.Giam_PhanTram.HasValue
                        ? tongTienGoc * voucher.Giam_PhanTram.Value / 100m
                        : Math.Min(voucher.Giam_Tien ?? 0, tongTienGoc);
                }
            }

            decimal tongThanhToan = tongTienGoc - giamGia;

            return _response.SetSuccess("Tính tiền thành công", new { TongThanhToan = tongThanhToan });
        }

        private async Task Sel(List<CheckoutItem> chiTiet)
        {

            var selectedItems = await _context.ChiTietGioHangs
                .Where(x => chiTiet.Select(c => c.Ma_BienThe).Contains(x.Ma_BienThe))
                .ToListAsync();

            if (!selectedItems.Any())
                return;

            foreach(var item in selectedItems)
            {
                item.IsChecked = true;

                _context.ChiTietGioHangs.Update(item);
            }
            await _context.SaveChangesAsync();

        }
        // Request model


        public class CheckoutItem
        {
            public int Ma_BienThe { get; set; }
            public int So_Luong { get; set; }
        }
    }
}
