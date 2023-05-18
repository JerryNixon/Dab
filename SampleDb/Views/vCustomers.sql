CREATE VIEW [dbo].[vCustomers] AS
SELECT *
		, UPPER(Name) AS UpperName 
FROM Customers
