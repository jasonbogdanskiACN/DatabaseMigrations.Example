CREATE PROCEDURE [dbo].[GetUsersByEmail]
    @Email nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [Id],[Name],[Email] FROM [Users] WHERE [Email] = @Email;
END
