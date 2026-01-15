using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfMigrations.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("EF Core Migrations Example (SQL Server)");

            using var db = new AppDbContext();

            Console.WriteLine("Applying pending migrations...");
            db.Database.Migrate();
            Console.WriteLine("Migrations applied. Current Users in DB:");

            foreach (var u in db.Users)
            {
                Console.WriteLine($"- {u.Id}: {u.Name} ({u.Email})");
            }

            Console.WriteLine("Adding a new user and saving changes...");
            db.Users.Add(new User { Name = "Alice", Email = "alice@example.com", PhoneNumber = "3308083213" });
            db.SaveChanges();

            Console.WriteLine("Users after insert:");
            foreach (var u in db.Users)
            {
                Console.WriteLine($"- {u.Id}: {u.Name} ({u.Email})");
            }

            // Example: call stored procedure added via EF Core migration
            Console.WriteLine("Querying users by email using stored procedure GetUsersByEmail...");
            var email = "alice@example.com";

            // Use FromSqlInterpolated to call the stored procedure and materialize results into the User entity
            var usersByEmail = db.Users
                .FromSqlInterpolated($"EXEC [dbo].[GetUsersByEmail] {email}")
                .ToList();

            Console.WriteLine($"Users returned by stored procedure for '{email}':");
            foreach (var u in usersByEmail)
            {
                Console.WriteLine($"- {u.Id}: {u.Name} ({u.Email})");
            }

            Console.WriteLine("Done.");
        }
    }
}
