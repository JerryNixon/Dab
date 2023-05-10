-- setup security

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'DabLogin')
BEGIN
    CREATE LOGIN DabLogin WITH PASSWORD = 'P@ssw0rd!';
END

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DabUser')
BEGIN
    CREATE USER [DabUser] FOR LOGIN [DabLogin];
END

EXEC sp_addrolemember 'db_owner', 'DabUser'