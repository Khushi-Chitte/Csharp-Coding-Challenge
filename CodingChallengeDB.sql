create database CodingChallengeDB;
GO

use CodingChallengeDB;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY,
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    Role NVARCHAR(50)
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    Description NVARCHAR(255),
    Price DECIMAL(10, 2),
    QuantityInStock INT,
    Type NVARCHAR(50),
    Brand NVARCHAR(100),         
    WarrantyPeriod INT,          
    Size NVARCHAR(50),           
    Color NVARCHAR(50)           
);


CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1, 1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY IDENTITY(1, 1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT
);
select * from Users
select * from Products