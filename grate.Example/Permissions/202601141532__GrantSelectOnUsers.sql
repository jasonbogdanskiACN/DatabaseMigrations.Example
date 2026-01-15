-- Everytime script: grant select on Users to role
IF OBJECT_ID('dbo.Users','U') IS NOT NULL
BEGIN
    GRANT SELECT ON [dbo].[Users] TO [public];
END
