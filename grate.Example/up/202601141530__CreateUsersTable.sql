-- runAfterCreateDatabase (one-time) script
CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(200) NULL,
    [Phone] NVARCHAR(20) NULL
);
