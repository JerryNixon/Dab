CREATE TABLE Customers
(
      Id INT PRIMARY KEY IDENTITY(1, 1)
    , Name NVARCHAR(50) NULL
    , City VARCHAR(MAX) NOT NULL
    , State VARCHAR(50) NOT NULL DEFAULT 'CO'
    , Location AS (City + ', ' + State) PERSISTED
    , SpecialRank DECIMAL(18, 2) NOT NULL DEFAULT 0
    , weirdName VARCHAR(10)
    , [Very Weird Name] VARCHAR(10)
)