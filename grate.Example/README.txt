grate.Example folder

This folder shows a minimal example of how to set up and run Grate database migrations.

Project: https://github.com/grate-devs/grate

Quick steps

1) Install Grate as a global dotnet tool (or install to a local manifest):
   # Global
   dotnet tool install --global grate

   # Local (recommended for teams - run in repository root)
   dotnet new tool-manifest
   dotnet tool install grate

2) Place your SQL migration files in `grate.Example/migrations/`.
   Files can be named with a numeric prefix to control order, e.g. `001__CreateUsersTable.sql`.

3) Run the provided script to apply migrations (example uses a connection string environment variable):
   # PowerShell
   ./grate.Example/run-grate-migrations.ps1 -ConnectionString "Server=(localdb)\\mssqllocaldb;Database=GrateExampleDb;Trusted_Connection=True;"

   # Bash
   ./grate.Example/run-grate-migrations.sh "Server=(localdb)\\mssqllocaldb;Database=GrateExampleDb;Trusted_Connection=True;"

Notes
- The provided scripts try to install Grate if it is not found and then invoke the CLI. The exact CLI flags may vary by Grate version; adjust the `grate` invocation in `run-grate-migrations.*` to match the installed version. See `grate --help` for available commands.
