using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfMigrations.Example.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetUsersByEmailStoredProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"ALTER PROCEDURE [dbo].[GetUsersByEmail]
    @Email nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [Id],[Name],[Email],[PhoneNumber] FROM [Users] WHERE [Email] = @Email;
END";

            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetUsersByEmail]
    @Email nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [Id],[Name],[Email] FROM [Users] WHERE [Email] = @Email;
END";

            migrationBuilder.Sql(sp);
        }
    }
}
