using FashionStore.DTO;
using FashionStore.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace FashionStore.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSetings;
        public EmailService(IOptions<EmailSettings> emailSetings)
        {
            _emailSetings = emailSetings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = new SmtpClient(_emailSetings.SmtpServer, _emailSetings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSetings.SenderEmail, _emailSetings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_emailSetings.SenderEmail, to, subject, body);
            await client.SendMailAsync(mailMessage);
        }

        public async Task SendWelcomeEmailAsync(string to, string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Email không được để trống!", nameof(to));

            string subject = "Chào mừng đến với FashionStore!";
            string body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <table width='100%' style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 20px;'>
        <tr>
            <td style='text-align: center;'>
                <img src='https://yourdomain.com/logo.png' alt='FashionStore Logo' width='150' style='margin-bottom: 20px;'/>
            </td>
        </tr>
        <tr>
            <td>
                <h2 style='color: #333333;'>Xin chào {tenDangNhap},</h2>
                <p>Cảm ơn bạn đã đăng ký tài khoản tại <b>FashionStore</b>!</p>
                <p>Chúc bạn có trải nghiệm tuyệt vời khi sử dụng dịch vụ của chúng tôi.</p>
                <hr style='border: none; border-top: 1px solid #dddddd; margin: 20px 0;'/>
                <p style='font-size: small; color: #777777;'>Đây là email tự động từ FashionStore, vui lòng không trả lời.</p>
            </td>
        </tr>
    </table>
</body>
</html>
";

            var mailMessage = new MailMessage(_emailSetings.SenderEmail, to, subject, body)
            {
                IsBodyHtml = true
            };

            var client = new SmtpClient(_emailSetings.SmtpServer, _emailSetings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSetings.SenderEmail, _emailSetings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(mailMessage);
        }
        public async Task SendAccountUpdatedEmailAsync(string to, string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Email không được để trống!", nameof(to));

            string subject = "Cập nhật tài khoản FashionStore";

            string body = $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
            <table width='100%' style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 20px;'>
                <tr>
                    <td style='text-align: center;'>
                        <img src='https://yourdomain.com/logo.png' alt='FashionStore Logo' width='150' style='margin-bottom: 20px;'/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h2 style='color: #333333;'>Xin chào {tenDangNhap},</h2>
                        <p>Tài khoản của bạn tại <b>FashionStore</b> vừa được cập nhật thông tin thành công!</p>
                        <p>Nếu bạn không thực hiện thay đổi này, vui lòng liên hệ ngay với chúng tôi để được hỗ trợ.</p>
                        <hr style='border: none; border-top: 1px solid #dddddd; margin: 20px 0;'/>
                        <p style='font-size: small; color: #777777;'>Đây là email tự động từ FashionStore, vui lòng không trả lời.</p>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        ";

            var mailMessage = new MailMessage(_emailSetings.SenderEmail, to, subject, body)
            {
                IsBodyHtml = true
            };

            var client = new SmtpClient(_emailSetings.SmtpServer, _emailSetings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSetings.SenderEmail, _emailSetings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(mailMessage);
        }
        public async Task SendOrderEmailAsync(string to, DonHangDTO donHang)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Email không được để trống!", nameof(to));

            string subject = $"Xác nhận đơn hàng {donHang.Ma_DonHang} - FashionStore";

            string tableRows = "";
            foreach (var item in donHang.ChiTiet)
            {
                string hinhAnh = string.IsNullOrEmpty(item.Hinh_Anh)
                    ? "https://yourdomain.com/images/no-image.jpg"
                    : item.Hinh_Anh;

                tableRows += $@"
<tr>
    <td style='padding: 12px; border-bottom: 1px solid #eee; vertical-align: top;'>
        <img src='{hinhAnh}' alt='{item.Ten_SanPham}' width='80' style='border-radius: 8px; float: left; margin-right: 10px;' />
        <div>
            <strong>{item.Ten_SanPham}</strong><br>
            <small style='color: #666;'>
                Màu: {item.Mau_Sac ?? "Không có"} - 
                Size: {item.Kich_Thuoc ?? "Freesize"}
            </small>
        </div>
    </td>
    <td style='padding: 12px; text-align: center; border-bottom: 1px solid #eee;'>
        {item.So_Luong}
    </td>
    <td style='padding: 12px; text-align: right; border-bottom: 1px solid #eee;'>
        {item.DonGia:N0}đ
    </td>
    <td style='padding: 12px; text-align: right; border-bottom: 1px solid #eee; font-weight: bold;'>
        {item.ThanhTien:N0}đ
    </td>
</tr>";
            }

            string body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
    <table width='100%' style='max-width: 700px; margin: auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.1);'>
        <tr>
            <td style='background: linear-gradient(135deg, #e91e63, #ff6b9d); padding: 30px; text-align: center; color: white;'>
                <h1 style='margin: 0; font-size: 28px;'>FASHION STORE</h1>
                <p style='margin: 10px 0 0; font-size: 16px;'>Cảm ơn bạn đã tin tưởng!</p>
            </td>
        </tr>
        <tr>
            <td style='padding: 30px;'>
                <h2 style='color: #e91e63;'>Đơn hàng của bạn đã được xác nhận!</h2>
                <p><strong>Mã đơn hàng:</strong> <span style='font-size: 20px; color: #e91e63;'>{donHang.Ma_DonHang}</span></p>
                <p><strong>Ngày đặt:</strong> {donHang.Ngay_Dat:dd/MM/yyyy HH:mm}</p>
                <p><strong>Hình thức thanh toán:</strong> {donHang.Ten_PhuongThuc}</p>

                <table style='width: 100%; margin: 20px 0; border-collapse: separate; border-spacing: 0 10px;'>
                    <thead>
                        <tr style='background: #fdf2f8; text-align: left;'>
                            <th style='padding: 15px; border-radius: 8px 0 0 8px;'>Sản phẩm</th>
                            <th style='padding: 15px; text-align: center;'>SL</th>
                            <th style='padding: 15px; text-align: right;'>Giá</th>
                            <th style='padding: 15px; text-align: right; border-radius: 0 8px 8px 0;'>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}
                    </tbody>
                </table>

                {(!string.IsNullOrEmpty(donHang.Ma_Voucher) ?
                            $"<p style='background: #fff8e1; padding: 15px; border-radius: 8px;'><strong>Mã giảm giá đã áp dụng:</strong> {donHang.Ma_Voucher}</p>" : "")}

                <div style='text-align: right; margin-top: 20px; padding: 20px; background: #f8f9fa; border-radius: 12px;'>
                    <h2 style='color: #e91e63; margin: 0;'>Tổng thanh toán: {donHang.Tong_Tien:N0}đ</h2>
                    <p style='margin: 10px 0 0; color: #666; font-size: 14px;'>Vui lòng chuẩn bị đúng số tiền khi nhận hàng nhé!</p>
                </div>

                <hr style='border: none; border-top: 2px dashed #eee; margin: 30px 0;'/>
                <p style='text-align: center; color: #999; font-size: 13px;'>
                    Hotline hỗ trợ: <strong>1900 9999</strong> • Email: support@fashionstore.vn<br>
                    Đây là email tự động, vui lòng không trả lời.
                </p>
            </td>
        </tr>
    </table>
</body>
</html>";

            var mailMessage = new MailMessage(_emailSetings.SenderEmail, to, subject, body)
            {
                IsBodyHtml = true
            };

            var client = new SmtpClient(_emailSetings.SmtpServer, _emailSetings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSetings.SenderEmail, _emailSetings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(mailMessage);
        }

    }
}
