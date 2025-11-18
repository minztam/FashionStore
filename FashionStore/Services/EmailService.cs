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

            // Tạo bảng HTML cho chi tiết sản phẩm
            string tableRows = "";
            foreach (var item in donHang.ChiTiet)
            {
                tableRows += $@"
<tr>
    <td style='padding: 8px; border: 1px solid #ddd;'>{item.Ten_SanPham}</td>
    <td style='padding: 8px; border: 1px solid #ddd;'>{item.So_Luong}</td>
    <td style='padding: 8px; border: 1px solid #ddd;'>{item.DonGia:C}</td>
    <td style='padding: 8px; border: 1px solid #ddd;'>{item.ThanhTien:C}</td>
</tr>";
            }

            string body = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <table width='100%' style='max-width: 700px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 20px;'>
        <tr>
            <td style='text-align: center;'>
                <img src='https://yourdomain.com/logo.png' alt='FashionStore Logo' width='150' style='margin-bottom: 20px;'/>
            </td>
        </tr>
        <tr>
            <td>
                <h2>Xin chào khách hàng,</h2>
                <p>Cảm ơn bạn đã đặt hàng tại <b>FashionStore</b>. Thông tin đơn hàng của bạn như sau:</p>

                <p><b>Mã đơn hàng:</b> {donHang.Ma_DonHang}</p>
                <p><b>Ngày đặt:</b> {donHang.Ngay_Dat:dd/MM/yyyy HH:mm}</p>
                <p><b>Trạng thái:</b> {donHang.Trang_Thai}</p>

                <table style='border-collapse: collapse; width: 100%; margin-top: 15px;'>
                    <thead>
                        <tr>
                            <th style='padding: 8px; border: 1px solid #ddd;'>Sản phẩm</th>
                            <th style='padding: 8px; border: 1px solid #ddd;'>Số lượng</th>
                            <th style='padding: 8px; border: 1px solid #ddd;'>Đơn giá</th>
                            <th style='padding: 8px; border: 1px solid #ddd;'>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}
                    </tbody>
                </table>

                <p style='text-align: right; font-weight: bold; margin-top: 15px;'>Tổng tiền: {donHang.Tong_Tien:C}</p>

                {(!string.IsNullOrEmpty(donHang.Ma_Voucher) ? $"<p>Voucher áp dụng: {donHang.Ma_Voucher}</p>" : "")}

                <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'/>
                <p style='font-size: small; color: #777;'>Đây là email tự động từ FashionStore, vui lòng không trả lời.</p>
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
