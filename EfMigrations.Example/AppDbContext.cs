using Microsoft.EntityFrameworkCore;

namespace EfMigrations.Example
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQL Server Express LocalDB instance by default. Adjust connection string as needed.
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=EfMigrationsExampleDb;User Id=sa;Password=YourStrong!Passw0rd123;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(u => u.Name).IsRequired().HasMaxLength(200);
                b.Property(u => u.Email).HasMaxLength(200);
            });
        }
    }
}
