-- RunAfterOtherAnyTimeScripts (Anytime) - seed data
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Email = 'alice@example.com')
BEGIN
    INSERT INTO [dbo].[Users] ([Name], [Email]) VALUES ('Alice', 'alice@example.com');
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Email = 'bob@example.com')
BEGIN
    INSERT INTO [dbo].[Users] ([Name], [Email]) VALUES ('Bob', 'bob@example.com');
END
