CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

INSERT INTO dbo.Users (Username, Email) VALUES ('alice', 'alice@example.com');
INSERT INTO dbo.Users (Username, Email) VALUES ('bob', 'bob@example.com');
INSERT INTO dbo.Users (Username, Email) VALUES ('test', 'test@example.com');
