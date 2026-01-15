CREATE OR ALTER PROCEDURE [dbo].[GetUsersByEmail]
    @Email nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [Id],[Name] FROM [Users] WHERE [Email] = @Email;
END
