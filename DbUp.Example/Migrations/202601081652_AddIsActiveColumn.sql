-- Add IsActive column to Users
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = Object_ID(N'dbo.Users'))
BEGIN
    ALTER TABLE dbo.Users ADD IsActive BIT NOT NULL DEFAULT 1;
END
