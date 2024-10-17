USE TechShop;

-- 1. Write an SQL query to retrieve the names and emails of all customers.
SELECT (FirstName + ' ' + LastName) AS CustomerName, Email 
FROM Customers;

-- 2. Write an SQL query to list all orders with their order dates and corresponding customer names.
SELECT o.OrderID, o.OrderDate, (c.FirstName + ' ' + c.LastName) AS CustomerName
FROM Orders o
INNER JOIN Customers c 
ON o.CustomerID = c.CustomerID;

-- 3. Write an SQL query to insert a new customer record into the "Customers" table. Include customer information such as name, email, and address.
INSERT INTO Customers (FirstName, LastName, Email, Phone, Address)
VALUES
('Ayush', 'Tripathi', 'ayuTri@example.in', 9988776655, 'Upperlake, Bhopal');

SELECT * FROM Customers where FirstName = 'Ayush';

-- 4. Write an SQL query to update the prices of all electronic gadgets in the "Products" table by increasing them by 10%.
UPDATE Products
SET PRICE = PRICE + PRICE * 0.1;

-- 5. Write an SQL query to delete a specific order and its associated order details from the "Orders" and "OrderDetails" tables. Allow users to input the order ID as a parameter.
ALTER TABLE Orders
DROP CONSTRAINT FK__Orders__Customer__4D94879B; --unique name assign by sql server

ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_CustomerID
FOREIGN KEY (CustomerID) REFERENCES Customers (CustomerID)
ON DELETE CASCADE;

GO

SP_HELP Orders;
GO

		--now orderdetails
ALTER TABLE OrderDetails
DROP CONSTRAINT FK__OrderDeta__Order__5070F446;

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_OrderID
FOREIGN KEY (OrderID) REFERENCES Orders (OrderID)
ON DELETE CASCADE;

ALTER TABLE OrderDetails  
DROP CONSTRAINT FK__OrderDeta__Produ__5165187F;

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_ProductID
FOREIGN KEY (ProductID) REFERENCES Products (ProductID)
ON DELETE CASCADE;

GO

SP_HELP OrderDetails;
GO
		--also adding on delete cascade to Inventory table
ALTER TABLE Inventory
DROP CONSTRAINT FK__Inventory__Produ__5441852A;

ALTER TABLE Inventory
ADD CONSTRAINT FK_Inventory_ProdutID
FOREIGN KEY (ProductID) REFERENCES Products (ProductID)
ON DELETE CASCADE;

GO

SP_HELP Inventory;
GO

DELETE FROM Orders
WHERE OrderID = 1010; --instead of using this OrderID users can input their own OrderID

SELECT * from Orders;
SELECT * from OrderDetails;

GO

-- 6. Write an SQL query to insert a new order into the "Orders" table. Include the customer ID, order date, and any other necessary information.
INSERT INTO Orders (CustomerID, OrderDate, TotalAmount)
VALUES
(11, GETDATE(), 550);

DECLARE @NewOrderID NUMERIC; -- declaring variable to get last orderId generated
SET @NewOrderID = SCOPE_IDENTITY();

INSERT INTO OrderDetails (OrderID, ProductID, Quantity)
VALUES
(@NewOrderID, 106, 1);

SELECT * from Orders;
SELECT * from OrderDetails;

--	7. Write an SQL query to update the contact information (e.g., email and address) of a specific customer in the "Customers" table. Allow users to input the customer ID and new contact information.
UPDATE Customers
SET Email = 'ayushTrip@example.com',
Phone = '8877665544'
WHERE CustomerID = 11;

-- 8.Write an SQL query to recalculate and update the total cost of each order in the "Orders" table based on the prices and quantities in the "OrderDetails" table.
UPDATE Orders
SET TotalAmount = (
	SELECT SUM(od.Quantity * p.Price)
	FROM OrderDetails od
	INNER JOIN Products p ON od.ProductID = p.ProductID
	WHERE od.OrderID = Orders.OrderID
);

SELECT * FROM Orders;

-- 9. Write an SQL query to delete all orders and their associated order details for a specific customer from the "Orders" and "OrderDetails" tables. Allow users to input the customer ID as a parameter.
DELETE FROM Orders
WHERE CustomerID = 7;

SELECT * FROM Orders;
SELECT * FROM OrderDetails; 

-- 10. Write an SQL query to insert a new electronic gadget product into the "Products" table, including product name, category, price, and any other relevant details.
INSERT INTO Products (ProductName, Description, Price) 
VALUES
('Stylus', 'A simple stylus to use with your phone', 100);

SELECT * FROM Products;

-- 11. Write an SQL query to update the status of a specific order in the "Orders" table (e.g., from "Pending" to "Shipped"). Allow users to input the order ID and the new status.
ALTER TABLE Orders
ADD Status VARCHAR(50) DEFAULT 'pending';

UPDATE Orders
SET Status = 'pending' 
WHERE Status IS NULL;

SELECT * FROM Orders;

UPDATE Orders
SET Status = 'shipped'
WHERE OrderID = 1011;

SELECT * FROM Orders;

-- 12. Write an SQL query to calculate and update the number of orders placed by each customer in the "Customers" table based on the data in the "Orders" table.
ALTER TABLE Customers
ADD OrdersCount NUMERIC DEFAULT 0;

UPDATE Customers
SET OrdersCount = 0
WHERE OrdersCount IS NULL;

UPDATE Customers
SET OrdersCount = (
	SELECT COUNT(*)
	FROM Orders o
	WHERE Customers.CustomerID = o.CustomerID
);

SELECT * FROM Customers;