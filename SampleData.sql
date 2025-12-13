-- =============================================
-- THÊM DỮ LIỆU MẪU ĐẦY ĐỦ ĐỂ TEST (V2 - FIXED)
-- =============================================

USE QuanLyBanHang;
GO

-- Xóa dữ liệu cũ
DELETE FROM InvoiceDetails;
DELETE FROM Invoices;
DELETE FROM Products;
DELETE FROM Categories;
DELETE FROM Users;
DELETE FROM Roles;
GO

-- =============================================
-- 1. ROLES
-- =============================================
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (RoleId, RoleName) VALUES (1, N'Admin');
INSERT INTO Roles (RoleId, RoleName) VALUES (2, N'Manager');
INSERT INTO Roles (RoleId, RoleName) VALUES (3, N'Staff');
SET IDENTITY_INSERT Roles OFF;
GO

-- =============================================
-- 2. USERS
-- =============================================
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (UserId, Username, PasswordHash, FullName, RoleId) VALUES 
(1, 'admin', '123456', N'Quản trị viên', 1),
(2, 'manager', '123456', N'Nguyễn Văn Quản Lý', 2),
(3, 'staff1', '123456', N'Trần Thị Nhân Viên 1', 3),
(4, 'staff2', '123456', N'Lê Văn Nhân Viên 2', 3),
(5, 'staff3', '123456', N'Phạm Thị Nhân Viên 3', 3);
SET IDENTITY_INSERT Users OFF;
GO

-- =============================================
-- 3. CATEGORIES
-- =============================================
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryId, CategoryName) VALUES 
(1, N'Đồ uống'),
(2, N'Thực phẩm'),
(3, N'Bánh kẹo'),
(4, N'Sữa & Sản phẩm từ sữa'),
(5, N'Đồ gia dụng'),
(6, N'Văn phòng phẩm');
SET IDENTITY_INSERT Categories OFF;
GO

-- =============================================
-- 4. PRODUCTS
-- =============================================
SET IDENTITY_INSERT Products ON;
-- Đồ uống
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(1, N'Coca Cola 330ml', 1, 100, 8000, 12000),
(2, N'Pepsi 330ml', 1, 80, 8000, 12000),
(3, N'Nước suối Aquafina 500ml', 1, 150, 4000, 8000),
(4, N'Trà xanh C2 500ml', 1, 60, 7000, 10000),
(5, N'Nước tăng lực RedBull', 1, 40, 12000, 18000),
(6, N'Sting dâu 330ml', 1, 70, 7000, 10000),
(7, N'Nước cam Teppy 1L', 1, 5, 20000, 28000);

-- Thực phẩm
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(8, N'Mì Hảo Hảo tôm chua cay', 2, 200, 3000, 5000),
(9, N'Mì Omachi xốt bò hầm', 2, 150, 6000, 9000),
(10, N'Phở bò Vifon', 2, 80, 7000, 10000),
(11, N'Cháo ăn liền Cháo Hạt', 2, 50, 8000, 12000),
(12, N'Xúc xích Vissan', 2, 3, 5000, 8000),
(13, N'Pate Hạ Long', 2, 45, 15000, 22000);

-- Bánh kẹo
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(14, N'Bánh Oreo', 3, 50, 15000, 22000),
(15, N'Bánh Chocopie', 3, 60, 25000, 35000),
(16, N'Kẹo cao su Big Babol', 3, 100, 3000, 5000),
(17, N'Bánh quy AFC', 3, 80, 8000, 12000),
(18, N'Snack Poca khoai tây', 3, 90, 7000, 10000),
(19, N'Kẹo dẻo Haribo', 3, 8, 18000, 25000);

-- Sữa
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(20, N'Sữa TH True Milk 180ml', 4, 120, 6000, 9000),
(21, N'Sữa Vinamilk 180ml', 4, 100, 5500, 8500),
(22, N'Sữa chua Vinamilk', 4, 80, 4000, 6000),
(23, N'Sữa đặc Ông Thọ', 4, 60, 12000, 18000),
(24, N'Phô mai Con Bò Cười', 4, 35, 25000, 35000);

-- Đồ gia dụng
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(25, N'Giấy vệ sinh Pulppy', 5, 50, 25000, 35000),
(26, N'Khăn giấy Tempo', 5, 70, 15000, 22000),
(27, N'Bàn chải đánh răng', 5, 40, 8000, 15000),
(28, N'Kem đánh răng Colgate', 5, 55, 18000, 28000),
(29, N'Dầu gội Clear 180ml', 5, 30, 30000, 45000);

-- Văn phòng phẩm
INSERT INTO Products (ProductId, ProductName, CategoryId, Quantity, CostPrice, SellingPrice) VALUES 
(30, N'Bút bi Thiên Long', 6, 200, 2000, 4000),
(31, N'Vở học sinh 96 trang', 6, 100, 8000, 12000),
(32, N'Thước kẻ 30cm', 6, 80, 3000, 5000),
(33, N'Tẩy Pentel', 6, 150, 2000, 3500),
(34, N'Bút highlight', 6, 60, 5000, 8000);
SET IDENTITY_INSERT Products OFF;
GO

-- =============================================
-- 5. INVOICES
-- =============================================
SET IDENTITY_INSERT Invoices ON;
INSERT INTO Invoices (InvoiceId, InvoiceDate, UserId, CustomerName, TotalAmount, FinalAmount) VALUES 
(1, DATEADD(day, -5, GETDATE()), 3, N'Nguyễn Văn A', 45000, 49500),
(2, DATEADD(day, -4, GETDATE()), 4, N'Trần Thị B', 85000, 93500),
(3, DATEADD(day, -3, GETDATE()), 3, N'Lê Văn C', 120000, 132000),
(4, DATEADD(day, -2, GETDATE()), 5, NULL, 35000, 38500),
(5, DATEADD(day, -1, GETDATE()), 4, N'Phạm Văn D', 156000, 171600),
(6, GETDATE(), 3, N'Hoàng Thị E', 67000, 73700);
SET IDENTITY_INSERT Invoices OFF;
GO

-- =============================================
-- 6. INVOICE DETAILS
-- =============================================
SET IDENTITY_INSERT InvoiceDetails ON;
-- Hóa đơn 1
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(1, 1, 1, 2, 12000, 24000),
(2, 1, 8, 3, 5000, 15000),
(3, 1, 30, 1, 4000, 4000);

-- Hóa đơn 2
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(4, 2, 14, 2, 22000, 44000),
(5, 2, 20, 3, 9000, 27000),
(6, 2, 3, 2, 8000, 16000);

-- Hóa đơn 3
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(7, 3, 5, 3, 18000, 54000),
(8, 3, 15, 2, 35000, 70000);

-- Hóa đơn 4
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(9, 4, 8, 5, 5000, 25000),
(10, 4, 4, 1, 10000, 10000);

-- Hóa đơn 5
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(11, 5, 25, 2, 35000, 70000),
(12, 5, 28, 2, 28000, 56000),
(13, 5, 29, 1, 45000, 45000);

-- Hóa đơn 6
INSERT INTO InvoiceDetails (DetailId, InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) VALUES
(14, 6, 1, 3, 12000, 36000),
(15, 6, 18, 2, 10000, 20000),
(16, 6, 22, 1, 6000, 6000);
SET IDENTITY_INSERT InvoiceDetails OFF;
GO

PRINT N'===========================================';
PRINT N'ĐÃ THÊM DỮ LIỆU MẪU ĐẦY ĐỦ!';
PRINT N'===========================================';
PRINT N'';
PRINT N'TÀI KHOẢN: admin, manager, staff1, staff2, staff3 / 123456';
PRINT N'';
PRINT N'- 6 Danh mục';
PRINT N'- 34 Sản phẩm';
PRINT N'- 6 Hóa đơn + 16 chi tiết';
PRINT N'- 3 SP tồn kho thấp: Nước cam (5), Xúc xích (3), Kẹo Haribo (8)';
GO
