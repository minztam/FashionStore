using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using FashionStore.Models;
using Microsoft.Extensions.Options;

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
    }
}
