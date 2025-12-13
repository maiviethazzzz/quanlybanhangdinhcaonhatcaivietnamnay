# HỆ THỐNG QUẢN LÝ BÁN HÀNG (POS) - Desktop

## Mục tiêu
Xây dựng hệ thống quản lý bán hàng hoàn chỉnh, đáp ứng yêu cầu quản lý tồn kho, giao dịch bán hàng, phân quyền người dùng.

## Cấu trúc thư mục (3 lớp)
```
QuanLyBanHang/
├── UI/         # Lớp giao diện (Forms, Pages)
├── BLL/        # Lớp nghiệp vụ (Business Logic)
├── DAL/        # Lớp truy cập dữ liệu (Data Access)
├── MODEL/      # Các đối tượng dữ liệu (Entities)
└── UTIL/       # Tiện ích
```

## Thiết kế CSDL (6 bảng)
1. **Roles** - Vai trò (Admin, Manager, Staff)
2. **Users** - Người dùng
3. **Categories** - Danh mục sản phẩm
4. **Products** - Sản phẩm (tên, đơn vị, giá nhập, giá bán, tồn kho)
5. **Invoices** - Hóa đơn
6. **InvoiceDetails** - Chi tiết hóa đơn

## Phân quyền người dùng
| Vai trò | Quyền hạn |
|---------|-----------|
| Admin | Toàn quyền: quản lý người dùng, sản phẩm, báo cáo |
| Manager | Quản lý sản phẩm, nhập hàng, báo cáo |
| Staff | Bán hàng, xem báo cáo cá nhân |

## Chức năng chính

### 1. Đăng nhập / Đăng xuất
- Kiểm tra username/password
- Phân quyền theo vai trò

### 2. Quản lý sản phẩm / kho hàng
- Thêm / Sửa / Xóa sản phẩm
- Quản lý danh mục
- Tồn kho tự động trừ khi bán
- Cảnh báo tồn kho thấp (< 10)
- Nhập hàng: cập nhật số lượng, giá nhập

### 3. Bán hàng (POS)
- Tìm kiếm sản phẩm
- Thêm vào giỏ hàng (double-click)
- Giảm giá theo %
- Tính VAT 10%
- Tính tiền thừa
- Lưu hóa đơn

### 4. Báo cáo thống kê
- Danh sách hóa đơn
- Doanh số theo nhân viên
- Doanh số theo ngày
- Cảnh báo tồn kho

## Hướng dẫn cài đặt

### 1. Tạo CSDL
- Mở SQL Server Management Studio
- Chạy file `Database_Script.sql`

### 2. Cấu hình kết nối
- Mở file `DAL/DatabaseHelper.cs`
- Sửa chuỗi kết nối phù hợp với máy bạn

### 3. Chạy ứng dụng
```bash
cd QuanLyBanHang
dotnet run
```

## Tài khoản đăng nhập
| Username | Password | Vai trò |
|----------|----------|---------|
| admin    | 123456   | Admin   |
| manager  | 123456   | Manager |
| staff    | 123456   | Staff   |

## Công nghệ sử dụng
- .NET 8 / WPF (Desktop)
- SQL Server
- Kiến trúc 3 lớp (UI - BLL - DAL)
