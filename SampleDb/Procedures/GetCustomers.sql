CREATE PROCEDURE PageCustomers
    @StartIndex INT = 1,
    @PageSize INT
AS
BEGIN
    SELECT [Id], [Name], [City], [State]
    FROM Customers
    ORDER BY Name
    OFFSET (@StartIndex - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END