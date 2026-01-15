namespace EfMigrations.Example
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
