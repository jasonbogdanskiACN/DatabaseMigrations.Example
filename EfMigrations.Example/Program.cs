using System;
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
            db.Users.Add(new User { Name = "Alice", Email = "alice@example.com" });
            db.SaveChanges();

            Console.WriteLine("Users after insert:");
            foreach (var u in db.Users)
            {
                Console.WriteLine($"- {u.Id}: {u.Name} ({u.Email})");
            }

            Console.WriteLine("Done.");
        }
    }
}
