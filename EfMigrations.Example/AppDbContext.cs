using Microsoft.EntityFrameworkCore;

namespace EfMigrations.Example
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQL Server Express LocalDB instance by default. Adjust connection string as needed.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EfMigrationsExampleDb;Trusted_Connection=True;MultipleActiveResultSets=true");
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
