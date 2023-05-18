CREATE TABLE Orders
(
    Id INT PRIMARY KEY,
    Date DATETIME,
    CustomerId INT NOT NULL,
    CONSTRAINT FK_Customer FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
)