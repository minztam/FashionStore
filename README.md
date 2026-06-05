## 🚀 Tổng quan

`FashionStore` là API backend được xây dựng trên ASP.NET Core 8.0, tập trung vào quy trình vận hành cửa hàng thời trang từ quản lý kho, xử lý đơn hàng đến tích hợp thanh toán điện tử.

## 🛠 Công nghệ sử dụng

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server / EF Core
- **Auth**: JWT (JSON Web Token)
- **Payments**: MoMo, VNPay
- **Features**: Real-time Order Tracking (giả lập), Email Service, Role-based Access Control (RBAC)

## 🧩 Cấu trúc dự án

```plaintext
FashionStore/
├── Controllers/       # API endpoints (RESTful)
├── Repositories/      # Data Access Layer
├── Services/          # Business Logic (Email, Payment, Auth)
├── Models/            # Database Entities
├── DTO/               # Request/Response objects
└── Library/           # VnPay/MoMo integration helpers
```

## 🛠 Hướng dẫn cài đặt

1. **Clone repo**:
   ```bash
   git clone <repo-url>
   ```
2. **Cấu hình DB**: Cập nhật `ConnectionStrings:FashionStoreDB` trong `appsettings.json`.
3. **Cập nhật database**:
   ```bash
   dotnet ef database update
   ```
4. **Chạy dự án**:
   ```bash
   dotnet run
   ```

## ⚙️ Các vai trò (Roles)

Hệ thống hỗ trợ phân quyền chặt chẽ thông qua JWT claims:

- `Admin`: Toàn quyền quản lý.
- `Nhân viên bán hàng`: Quản lý đơn hàng & sản phẩm.
- `Nhân viên kho`: Quản lý tồn kho.
- `Shipper`: Nhận đơn và cập nhật trạng thái vận chuyển.
- `Khách hàng`: Đặt hàng và theo dõi đơn hàng.

## 🔗 Endpoint nổi bật

- **Auth**: `POST /api/Auth/login` | `POST /api/Auth/register`
- **Sản phẩm**: `GET /api/SanPham` | `POST /api/SanPham` (Admin/Sale)
- **Đơn hàng**: `POST /api/DonHang/dat-hang` | `PATCH /api/DonHang/cap-nhat-trang-thai`
- **Shipper**: `POST /api/DonHang/gan-shipper-random/{maDonHang}`

## 📌 Lưu ý quan trọng

- Email service sử dụng cấu hình SMTP trong `appsettings.json`.
- JWT được cấu hình để xác thực và phân quyền cho tất cả endpoint.
- Dữ liệu mẫu được seed tự động khi tạo database.
- Swagger UI được bật trong môi trường Development để thử nghiệm API.

## 📂 Mô tả chi tiết cấu trúc

- `Program.cs` - cấu hình DI, middleware, authentication, session, Swagger và DbContext.
- `Data/FashionStoreContext.cs` - định nghĩa entity, quan hệ dữ liệu và seed data.
- `Controllers/` - chứa các API controller.
- `Repositories/` - triển khai pattern repository cho truy cập dữ liệu.
- `Services/` - chứa dịch vụ xử lý email, thanh toán, JWT.
- `Models/` - định nghĩa các entity cho database.
- `DTO/` - định nghĩa các đối tượng truyền dữ liệu giữa client và server.
- `Library/` - helper cho tích hợp VnPay/MoMo.

## 🌐 Hệ sinh thái dự án

Đây là giao diện Frontend của hệ thống **FashionStore**.

- **Frontend Repository**: [FashionStore](https://gitlab.com/prjfashinstore/feprj)
