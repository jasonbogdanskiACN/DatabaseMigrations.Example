This project uses EF Core migrations. To create migrations and update the database run the following commands from the EfMigrations.Example directory:

  dotnet tool install --global dotnet-ef --version 10.0.2
  dotnet ef migrations add InitialCreate
  dotnet ef database update

The Program will also apply any pending migrations at runtime using `Database.Migrate()`.
