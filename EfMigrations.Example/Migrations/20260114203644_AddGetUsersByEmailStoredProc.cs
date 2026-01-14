using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfMigrations.Example.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUsersByEmailStoredProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetUsersByEmail];");
        }
    }
}
