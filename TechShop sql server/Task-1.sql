-- 1. Create the database named "TechShop"
CREATE DATABASE TechShop;
GO

EXEC sp_databases;
GO

USE TechShop;
GO

-- 2. Define the schema for the Customers, Products, Orders, OrderDetails and Inventory tables based on the provided schema.
-- 4. Create appropriate Primary Key and Foreign Key constraints for referential integrity.
CREATE TABLE Customers (
	CustomerID NUMERIC PRIMARY KEY IDENTITY(1,1),
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	Email VARCHAR(150),
	Phone NUMERIC(10),
    Address VARCHAR(250)
);

CREATE TABLE Products (
	ProductID NUMERIC PRIMARY KEY IDENTITY(101,1),
	ProductName VARCHAR(100),
	Description	VARCHAR(500),
	Price MONEY
);

CREATE TABLE Orders (
	OrderID NUMERIC PRIMARY KEY IDENTITY(1001,1),
	CustomerID NUMERIC FOREIGN KEY REFERENCES Customers(CustomerID),
	OrderDate DATE,
	TotalAmount MONEY
);

CREATE TABLE OrderDetails (
    OrderDetailID NUMERIC PRIMARY KEY IDENTITY(10001,1),
    OrderID NUMERIC FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID NUMERIC FOREIGN KEY REFERENCES Products(ProductID),
    Quantity NUMERIC  
);

CREATE TABLE Inventory (
    InventoryID NUMERIC PRIMARY KEY IDENTITY(100001,1),
    ProductID NUMERIC FOREIGN KEY REFERENCES Products(ProductID),
    QuantityInStock NUMERIC,
    LastStockUpdate DATE
);
GO

SP_HELP Inventory;
GO

-- 5. Insert at least 10 sample records into each of the following tables.

	-- a. Customers
INSERT INTO Customers
VALUES
('Amit', 'Sharma', 'amit.sharma@example.com', 9876543210, '123 MG Road, Mumbai'),
('Priya', 'Patel', 'priya.patel@example.com', 9876543211, '456 Nehru Nagar, Ahmedabad'),
('Rohan', 'Kumar', 'rohan.kumar@example.com', 9876543212, '789 Residency Road, Bengaluru'),
('Sneha', 'Iyer', 'sneha.iyer@example.com', 9876543213, '321 Adyar, Chennai'),
('Rajesh', 'Verma', 'rajesh.verma@example.com', 9876543214, '654 Rajinder Nagar, New Delhi'),
('Anjali', 'Singh', 'anjali.singh@example.com', 9876543215, '987 Banjara Hills, Hyderabad'),
('Vikas', 'Gupta', 'vikas.gupta@example.com', 9876543216, '159 Park Street, Kolkata'),
('Meera', 'Nair', 'meera.nair@example.com', 9876543217, '753 Kowdiar, Thiruvananthapuram'),
('Arjun', 'Desai', 'arjun.desai@example.com', 9876543218, '258 MG Road, Pune'),
('Neha', 'Reddy', 'neha.reddy@example.com', 9876543219, '147 Jubilee Hills, Hyderabad');

SELECT * FROM Customers;

	-- b. Products
INSERT INTO Products (ProductName, Description, Price)
VALUES
('Smartphone X', 'High-end smartphone with 6.5-inch display', 800),
('Laptop Pro', 'Professional laptop with 16GB RAM and 512GB SSD', 1200),
('Wireless Earbuds', 'Noise-cancelling wireless earbuds', 130),
('Smartwatch Z', 'Water-resistant smartwatch with fitness tracking', 200),
('Tablet Plus', '10-inch tablet with stylus support', 500),
('Gaming Console', 'Next-gen gaming console with 4K support', 500),
('Bluetooth Speaker', 'Portable Bluetooth speaker with 20-hour battery life', 100),
('External Hard Drive', '1TB external hard drive', 60),
('4K Monitor', '27-inch 4K UHD monitor', 350),
('Wireless Keyboard', 'Slim wireless keyboard with backlight', 50);

SELECT * FROM Products;

	-- c. Orders
INSERT INTO Orders (CustomerID, OrderDate, TotalAmount)
VALUES
(1, '2024-09-10', 930),
(2, '2024-09-11', 550),
(3, '2024-09-12', 500),
(4, '2024-09-13', 2600),
(5, '2024-09-14', 800),
(6, '2024-09-15', 1490),
(7, '2024-09-16', 560),
(8, '2024-09-17', 650),
(9, '2024-09-18', 500),
(10, '2024-09-19', 600);

SELECT * FROM Orders;

	-- d. OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity)
VALUES
(1001, 101, 1),
(1001, 105, 1),
(1002, 106, 1),
(1002, 104, 1),
(1003, 106, 1),
(1004, 102, 2),
(1004, 107, 1),
(1005, 101, 1),
(1006, 101, 1),
(1006, 105, 1),
(1006, 104, 1),
(1007, 102, 1),
(1007, 107, 1),
(1007, 109, 1),
(1008, 108, 1),
(1008, 110, 1),
(1009, 106, 1),
(1010, 105, 1),
(1010, 104, 1);

SELECT * FROM OrderDetails;

	-- e. Inventory
INSERT INTO Inventory (ProductID, QuantityInStock, LastStockUpdate)
VALUES
(101, 50, '2024-09-01'),
(102, 30, '2024-09-05'),
(103, 100, '2024-09-02'),
(104, 25, '2024-09-10'),
(105, 60, '2024-09-08'),
(106, 45, '2024-09-06'),
(107, 200, '2024-09-04'),
(108, 150, '2024-09-07'),
(109, 75, '2024-09-03'),
(110, 120, '2024-09-09');

SELECT * FROM Inventory;








