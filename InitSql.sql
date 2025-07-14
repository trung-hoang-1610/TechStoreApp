-- Tạo database
CREATE DATABASE IF NOT EXISTS TechStore;
USE TechStore;
-- ------------------------
-- 1. Bảng quyền (admin / user)
-- ------------------------
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY AUTO_INCREMENT,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);
-- ------------------------
-- 2. Bảng người dùng
-- ------------------------
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    RoleId INT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- ------------------------
-- 3. Bảng danh mục sản phẩm
-- ------------------------
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY AUTO_INCREMENT,
    CategoryName VARCHAR(100) NOT NULL UNIQUE,
    Description TEXT
);
-- ------------------------
-- 4. Bảng nhà cung cấp 
-- ------------------------
CREATE TABLE Suppliers (
    SupplierId INT PRIMARY KEY AUTO_INCREMENT,
    SupplierName VARCHAR(100) NOT NULL,
    ContactInfo TEXT
);
-- ------------------------
-- 5. Bảng sản phẩm
-- ------------------------
CREATE TABLE Products (
    ProductId INT PRIMARY KEY AUTO_INCREMENT,
    ProductName VARCHAR(150) NOT NULL,
    Description TEXT,
    Unit VARCHAR(20),
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT DEFAULT 0, -- Số tồn kho hiện tại (có thể tính động từ giao dịch)
    MinStockLevel INT DEFAULT 0, -- Ngưỡng cảnh báo tồn kho
    CategoryId INT NOT NULL,
    SupplierId INT NOT NULL,
    CreatedBy INT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    IsActive  BOOLEAN DEFAULT 1, 
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
);

-- 6. Bảng phiếu nhập/xuất kho
CREATE TABLE StockTransactions (
    TransactionId INT PRIMARY KEY AUTO_INCREMENT,
    TransactionCode VARCHAR(50) NOT NULL UNIQUE,
    TransactionType ENUM('IN', 'OUT') NOT NULL,
    Note TEXT,
    CreatedBy INT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    ApprovedBy INT NOT NULL,
    ApprovedAt DATETIME,
    Status ENUM('Pending', 'Approved', 'Rejected') DEFAULT 'Pending',
    SupplierId INT NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    FOREIGN KEY (ApprovedBy) REFERENCES Users(UserId),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);

-- 7. Bảng chi tiết phiếu nhập/xuất
CREATE TABLE StockTransactionDetails (
    DetailId INT PRIMARY KEY AUTO_INCREMENT,
    TransactionId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Note TEXT,
    FOREIGN KEY (TransactionId) REFERENCES StockTransactions(TransactionId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

ALTER TABLE Suppliers
MODIFY COLUMN SupplierName VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
ALTER TABLE Categories
MODIFY COLUMN CategoryName VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
MODIFY COLUMN Description TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
ALTER TABLE Suppliers
MODIFY COLUMN SupplierName VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
ALTER TABLE Products
MODIFY COLUMN ProductName VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
MODIFY COLUMN Description TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
MODIFY COLUMN Unit VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- ------------------------
-- 7. Thêm dữ liệu mẫu 
-- ------------------------
INSERT INTO Roles (RoleName)
VALUES 
    ('Admin'),
    ('User');
    
    INSERT INTO Users (Username, PasswordHash, Email, RoleId)
VALUES 
('Admin', '123', 'admin@techstore.com', 1);
INSERT INTO Suppliers (SupplierName, ContactInfo) VALUES
('FPT Trading', 'contact@fpt.com.vn'),
('Thế Giới Số', 'sales@thegioiso.vn'),
('Nguyễn Kim', 'support@nguyenkim.vn'),
('Phong Vũ', 'info@phongvu.vn'),
('CellphoneS', 'cskh@cellphones.com.vn');


INSERT INTO Categories (CategoryName, Description) VALUES
('Laptop', 'Máy tính xách tay'),
('Điện thoại', 'Smartphones các loại'),
('Màn hình', 'Màn hình LCD/LED'),
('Phụ kiện', 'Chuột, bàn phím, tai nghe...'),
('Thiết bị mạng', 'Router, switch, thiết bị mạng khác');

INSERT INTO Products 
(ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt)
VALUES
-- CategoryId từ 1 đến 5, mỗi cái có 2 sản phẩm do mỗi Supplier nhập
-- Tổng cộng 5 Category * 5 Supplier * 2 sản phẩm = 50 sản phẩm

-- Laptop (CategoryId = 1)
('Laptop Asus A1', 'Laptop văn phòng', 'Chiếc', 15000000, 10, 2, 1, 1, 1, NOW()),
('Laptop Asus A2', 'Laptop văn phòng', 'Chiếc', 15500000, 12, 3, 1, 1, 1, NOW()),
('Laptop Dell D1', 'Laptop học sinh', 'Chiếc', 14000000, 8, 2, 1, 1, 1, NOW()),
('Laptop Dell D2', 'Laptop học sinh', 'Chiếc', 14200000, 5, 2, 1, 1, 1, NOW()),
('Laptop HP H1', 'Laptop đồ họa', 'Chiếc', 20000000, 7, 3, 1, 1, 1, NOW()),
('Laptop HP H2', 'Laptop đồ họa', 'Chiếc', 20500000, 6, 3, 1, 1, 1, NOW()),
('Laptop Lenovo L1', 'Laptop gaming', 'Chiếc', 18000000, 9, 2, 1, 1, 1, NOW()),
('Laptop Lenovo L2', 'Laptop gaming', 'Chiếc', 18500000, 10, 3, 1, 1, 1, NOW()),
('Laptop Macbook M1', 'Macbook Pro 2020', 'Chiếc', 32000000, 4, 1, 1, 1, 1, NOW()),
('Laptop Macbook M2', 'Macbook Air M2', 'Chiếc', 28000000, 5, 1, 1, 1, 1, NOW()),

-- Điện thoại (CategoryId = 2)
('iPhone 13', 'Apple iPhone 13 128GB', 'Chiếc', 18000000, 15, 4, 2, 2, 1, NOW()),
('iPhone 14', 'Apple iPhone 14 128GB', 'Chiếc', 20000000, 13, 4, 2, 2, 1, NOW()),
('Samsung A53', 'Samsung Galaxy A53', 'Chiếc', 9000000, 20, 5, 2, 2, 1, NOW()),
('Samsung S22', 'Samsung Galaxy S22', 'Chiếc', 19000000, 7, 2, 2, 2, 1, NOW()),
('Xiaomi Note 12', 'Xiaomi Redmi Note 12', 'Chiếc', 6000000, 11, 2, 2, 2, 1, NOW()),
('Xiaomi 13 Pro', 'Xiaomi 13 Pro 5G', 'Chiếc', 22000000, 3, 1, 2, 2, 1, NOW()),
('Oppo Reno 8', 'Oppo Reno 8 Pro', 'Chiếc', 10500000, 6, 2, 2, 2, 1, NOW()),
('Oppo A78', 'Oppo A78 4G', 'Chiếc', 7000000, 8, 2, 2, 2, 1, NOW()),
('Realme C55', 'Realme C55 6GB', 'Chiếc', 4500000, 10, 1, 2, 2, 1, NOW()),
('Realme GT Neo', 'Realme GT Neo 5G', 'Chiếc', 11000000, 4, 1, 2, 2, 1, NOW()),

-- Màn hình (CategoryId = 3)
('Dell 24"', 'Màn hình Dell 24 inch IPS', 'Chiếc', 3500000, 10, 2, 3, 3, 1, NOW()),
('Dell 27"', 'Màn hình Dell 27 inch 2K', 'Chiếc', 5500000, 6, 2, 3, 3, 1, NOW()),
('LG 24MP', 'Màn hình LG 24MP IPS', 'Chiếc', 3600000, 8, 1, 3, 3, 1, NOW()),
('LG UltraFine', 'Màn hình LG UltraFine 4K', 'Chiếc', 9000000, 3, 1, 3, 3, 1, NOW()),
('Samsung M5', 'Màn hình thông minh Samsung M5', 'Chiếc', 4000000, 5, 2, 3, 3, 1, NOW()),
('Samsung M7', 'Màn hình thông minh Samsung M7', 'Chiếc', 7000000, 4, 2, 3, 3, 1, NOW()),
('Asus VG245', 'Màn hình gaming Asus VG245', 'Chiếc', 4800000, 7, 2, 3, 3, 1, NOW()),
('Asus TUF', 'Màn hình Asus TUF Gaming 2K', 'Chiếc', 6900000, 2, 1, 3, 3, 1, NOW()),
('AOC 24B1X', 'Màn hình AOC 24 inch', 'Chiếc', 3100000, 6, 2, 3, 3, 1, NOW()),
('AOC Q27G2S', 'Màn hình AOC 2K gaming', 'Chiếc', 5600000, 3, 1, 3, 3, 1, NOW()),

-- Phụ kiện (CategoryId = 4)
('Logitech B100', 'Chuột Logitech B100', 'Chiếc', 150000, 50, 10, 4, 4, 1, NOW()),
('Logitech K120', 'Bàn phím Logitech K120', 'Chiếc', 250000, 30, 5, 4, 4, 1, NOW()),
('Razer DeathAdder', 'Chuột gaming Razer', 'Chiếc', 900000, 10, 2, 4, 4, 1, NOW()),
('Razer BlackWidow', 'Bàn phím cơ Razer', 'Chiếc', 2200000, 4, 1, 4, 4, 1, NOW()),
('HyperX Cloud', 'Tai nghe gaming HyperX', 'Chiếc', 1500000, 6, 2, 4, 4, 1, NOW()),
('HyperX Alloy', 'Bàn phím cơ HyperX Alloy', 'Chiếc', 1800000, 5, 1, 4, 4, 1, NOW()),
('Fuhlen L102', 'Chuột Fuhlen giá rẻ', 'Chiếc', 100000, 40, 5, 4, 4, 1, NOW()),
('Fuhlen M87S', 'Bàn phím cơ Fuhlen', 'Chiếc', 650000, 12, 2, 4, 4, 1, NOW()),
('Corsair HS35', 'Tai nghe Corsair HS35', 'Chiếc', 990000, 5, 1, 4, 4, 1, NOW()),
('Corsair K70', 'Bàn phím Corsair K70', 'Chiếc', 2600000, 3, 1, 4, 4, 1, NOW()),

-- Thiết bị mạng (CategoryId = 5)
('TP-Link TL840N', 'Router Wi-Fi TP-Link TL840N', 'Chiếc', 450000, 20, 3, 5, 5, 1, NOW()),
('TP-Link Archer C6', 'Router TP-Link C6 AC1200', 'Chiếc', 750000, 12, 2, 5, 5, 1, NOW()),
('Tenda AC6', 'Router Tenda AC6', 'Chiếc', 590000, 15, 2, 5, 5, 1, NOW()),
('Tenda F3', 'Router Tenda F3 Wi-Fi', 'Chiếc', 370000, 10, 2, 5, 5, 1, NOW()),
('Mercusys MW325R', 'Router Mercusys 300Mbps', 'Chiếc', 320000, 18, 3, 5, 5, 1, NOW()),
('Mercusys AC12G', 'Router Mercusys AC1200', 'Chiếc', 620000, 9, 2, 5, 5, 1, NOW()),
('ASUS RT-AC59U', 'Router Asus băng tần kép', 'Chiếc', 980000, 6, 1, 5, 5, 1, NOW()),
('ASUS RT-N12+', 'Router Asus RT-N12+', 'Chiếc', 520000, 11, 1, 5, 5, 1, NOW()),
('D-Link DIR-612', 'Router D-Link DIR-612 N300', 'Chiếc', 490000, 14, 3, 5, 5, 1, NOW()),
('D-Link DIR-615', 'Router D-Link DIR-615 300Mbps', 'Chiếc', 450000, 13, 3, 5, 5, 1, NOW());



