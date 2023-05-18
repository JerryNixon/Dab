CREATE TABLE Lines
(
    Id INT PRIMARY KEY,
    Quantity INT NOT NULL,
    ProductId INT NOT NULL,
    OrderId INT NOT NULL,
    PriceEach MONEY NOT NULL,
    CONSTRAINT FK_Order FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    CONSTRAINT FK_Product FOREIGN KEY (ProductId) REFERENCES Products(Id)
)