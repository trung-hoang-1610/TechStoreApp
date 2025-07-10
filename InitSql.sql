
CREATE DATABASE IF NOT EXISTS TechStore;
USE TechStore;
-- ------------------------
-- Bảng quyền (admin / user)
-- ------------------------
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY AUTO_INCREMENT,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

-- ------------------------
-- Bảng người dùng
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
-- Bảng danh mục sản phẩm
-- ------------------------
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY AUTO_INCREMENT,
    CategoryName VARCHAR(100) NOT NULL UNIQUE,
    Description TEXT
);

-- ------------------------
-- Bảng sản phẩm công nghệ
-- ------------------------
CREATE TABLE Products (
    ProductId INT PRIMARY KEY AUTO_INCREMENT,
    ProductName VARCHAR(150) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT DEFAULT 0,
    CategoryId INT NOT NULL,
    CreatedBy INT, -- UserId người tạo sản phẩm
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
);

-- Thêm quyền
INSERT INTO Roles (RoleName)
VALUES 
    ('Admin'),
    ('User');
    
    -- Thêm danh mục sản phẩm
INSERT INTO Categories (CategoryName, Description)
VALUES 
    ('Tablet', 'Máy tính bảng'),
    ('Monitor', 'Màn hình máy tính'),
    ('Accessory', 'Phụ kiện công nghệ'),
    ('Camera', 'Thiết bị ghi hình'),
    ('Smartwatch', 'Đồng hồ thông minh');
    
-- SHA256 của "123456" = "8d969eef6ecad3c29a3a629280e686cff8fab9841aef..."

INSERT INTO Users (Username, PasswordHash, Email, RoleId)
VALUES 
    ('admin02', '8d969eef6ecad3c29a3a629280e686cff8fab9841aef...', 'admin2@example.com', 1),
    ('user01',  '8d969eef6ecad3c29a3a629280e686cff8fab9841aef...', 'user1@example.com', 2);
    truncate table Users
-- 20 sản phẩm
INSERT INTO Products (ProductName, Description, Price, Quantity, CategoryId, CreatedBy)
VALUES 
('MacBook Air M2', 'Laptop Apple siêu mỏng', 1999.99, 10, 1, 1),
('Dell XPS 13', 'Laptop Windows cao cấp', 1599.00, 8, 1, 1),
('HP Spectre x360', 'Laptop 2-trong-1 cao cấp', 1399.50, 12, 1, 1),
('Asus ROG Strix', 'Laptop gaming mạnh mẽ', 1799.99, 5, 1, 1),

('iPhone 15 Pro', 'Flagship mới của Apple', 1199.00, 15, 2, 1),
('Samsung Galaxy S24', 'Điện thoại Android cao cấp', 1099.00, 20, 2, 1),
('Xiaomi 13T Pro', 'Điện thoại giá tốt hiệu năng cao', 699.99, 30, 2, 1),
('Google Pixel 8', 'Camera AI ấn tượng', 999.00, 10, 2, 1),

('Chuột Logitech MX Master 3S', 'Chuột không dây tốt nhất', 99.99, 25, 3, 1),
('Bàn phím Keychron K6', 'Bàn phím cơ không dây', 89.00, 20, 3, 1),
('Đế tản nhiệt laptop', 'Phụ kiện hỗ trợ tản nhiệt', 29.99, 50, 3, 1),
('Cáp sạc USB-C Anker', 'Cáp nhanh và bền', 15.99, 100, 3, 1),

('Tai nghe Sony WH-1000XM4', 'Chống ồn chủ động tuyệt vời', 349.00, 18, 4, 1),
('Tai nghe AirPods Pro 2', 'Tai nghe Apple chống ồn', 249.00, 22, 4, 1),
('Loa JBL Charge 5', 'Loa bluetooth di động', 179.00, 14, 4, 1),
('Loa Marshall Emberton', 'Loa cổ điển kiêm hiện đại', 199.99, 9, 4, 1),

('Apple Watch Series 9', 'Thiết bị đeo tay thông minh Apple', 399.00, 11, 5, 1),
('Samsung Galaxy Watch 6', 'Đồng hồ thông minh Android', 349.00, 15, 5, 1),
('Mi Band 8', 'Vòng đeo tay thông minh giá rẻ', 39.99, 80, 5, 1),
('Garmin Venu 2', 'Đồng hồ thể thao chuyên nghiệp', 429.00, 6, 5, 1);


select * from Roles;
select * from Users;
select * from Categories;
select * from Products


