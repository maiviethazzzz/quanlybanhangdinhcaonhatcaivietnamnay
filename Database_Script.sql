

CREATE DATABASE QuanLyBanHang;
GO

USE QuanLyBanHang;
GO

-- 1. Bảng Roles (Vai trò)
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- 2. Bảng Users (Người dùng)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- 3. Bảng Categories (Danh mục)
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);

-- 4. Bảng Products (Sản phẩm)
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(200) NOT NULL,
    CategoryId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    CostPrice DECIMAL(10, 2) NOT NULL,
    SellingPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- 5. Bảng Invoices (Hóa đơn)
CREATE TABLE Invoices (
    InvoiceId INT PRIMARY KEY IDENTITY(1,1),
    InvoiceDate DATETIME NOT NULL DEFAULT GETDATE(),
    UserId INT NOT NULL,
    CustomerName NVARCHAR(100) NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    FinalAmount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- 6. Bảng InvoiceDetails (Chi tiết hóa đơn)
CREATE TABLE InvoiceDetails (
    DetailId INT PRIMARY KEY IDENTITY(1,1),
    InvoiceId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    LineTotal DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

GO

-- =============================================
-- DỮ LIỆU MẪU
-- =============================================

-- Thêm vai trò
INSERT INTO Roles (RoleName) VALUES (N'Admin');
INSERT INTO Roles (RoleName) VALUES (N'Manager');
INSERT INTO Roles (RoleName) VALUES (N'Staff');

-- Thêm người dùng (password: 123456)
INSERT INTO Users (Username, PasswordHash, FullName, RoleId) 
VALUES ('admin', '123456', N'Quản trị viên', 1);
INSERT INTO Users (Username, PasswordHash, FullName, RoleId) 
VALUES ('manager', '123456', N'Quản lý', 2);
INSERT INTO Users (Username, PasswordHash, FullName, RoleId) 
VALUES ('staff', '123456', N'Nhân viên', 3);

-- Thêm danh mục
INSERT INTO Categories (CategoryName) VALUES (N'Đồ uống');
INSERT INTO Categories (CategoryName) VALUES (N'Thực phẩm');
INSERT INTO Categories (CategoryName) VALUES (N'Bánh kẹo');

-- Thêm sản phẩm
INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
VALUES (N'Coca Cola 330ml', 1, 100, 8000, 12000);
INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
VALUES (N'Pepsi 330ml', 1, 80, 8000, 12000);
INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
VALUES (N'Mì Hảo Hảo', 2, 200, 3000, 5000);
INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
VALUES (N'Bánh Oreo', 3, 50, 15000, 22000);
INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
VALUES (N'Nước suối Aquafina 500ml', 1, 150, 4000, 8000);

GO

PRINT N'Tạo CSDL thành công!';
PRINT N'Đăng nhập: admin / 123456';
