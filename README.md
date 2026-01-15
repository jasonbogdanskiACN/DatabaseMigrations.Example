# DatabaseMigrations.Example
*A practical example of automated SQL Server database migrations using DbUp (with optional Grate & Flyway guidance), EF Core, and GitHub Actions*

This repository demonstrates a clean, maintainable approach for managing **SQL Server database schema changes** in a .NET application using:

- **DbUp.Example** — the console app that applies SQL migrations (primary migration engine in this repo)
- **Grate** — optional filesystem‑driven alternative
- **Flyway** — optional industry‑standard migration engine
- **EF Core** — for application data modeling (not schema deployment)
- **GitHub Actions** — to validate migrations automatically in CI using a real SQL Server container

This project is ideal for teams wanting predictable, script‑based database migrations that work locally, in CI, and in production.

**Note: Hardcoded database passwords would be moved to a secret provider in a production. This repo only serves as an example of database migration technologies**
---

## 🚀 Features

### ✔ Automated SQL migrations via DbUp.Example
The `DbUp.Example` console application:

- Executes SQL scripts from `/database/migrations`
- Tracks applied scripts using DbUp’s version table
- Applies scripts in deterministic order (timestamp filenames recommended)
- Runs both locally and in CI

### ✔ EF Core configured safely
EF Core is included for runtime data access — *not* for schema deployment.

The project includes:

- `AppDbContext` with DI‑friendly configuration
- A design‑time factory enabling `dotnet ef` commands
- Safe fallback connection handling in `OnConfiguring`

### ✔ GitHub Actions CI with SQL Server container
Your CI workflow:

1. Starts SQL Server 2022 in Docker
2. Waits for full readiness
3. Builds your .NET solution
4. Runs migrations using `DbUp.Example`
5. Fails the PR if any migration script fails

This ensures migrations are always valid before merging.

### ✔ Optional support for Grate and Flyway
While DbUp is the tool used in this repo, the SQL structure is compatible with:

- **Grate** — for teams preferring convention‑based folder layouts
- **Flyway** — for cross‑platform Java‑style migration workflows

Instructions for integrating each tool are included below.

---

## 🧰 Local Development

### 1) Start SQL Server in Docker
```bash
docker-compose up -d
```

### 2) Run migrations using DbUp.Example
```bash
dotnet run --project DbUp.Example   "Server=localhost,1433;Database=DbUpExample;User Id=sa;Password=YourStrong!Passw0rd123;TrustServerCertificate=True"
```

This will:
- Create the database if missing
- Create DbUp’s tracking table
- Apply all outstanding migration scripts in `/database/migrations`

---

## ✍️ Adding New Migration Scripts
1. Create a new `.sql` file inside:
```
/database/migrations
```

2. Use a timestamp prefix for ordering, e.g.:
```
20260115_1410_AddPriceColumn.sql
```

3. Add SQL, for example:
```sql
ALTER TABLE Users ADD Price DECIMAL(18,2) NOT NULL DEFAULT 0;
```

4. Commit & push — CI will validate automatically.

---

## 🧪 GitHub Actions CI Pipeline

Your CI workflow automatically:

- Spins up SQL Server 2022
- Waits for readiness
- Builds your solution
- Executes DbUp migrations via `DbUp.Example`
- Fails PRs if migrations break

### Required GitHub Secret
Create a secret in:
**Settings → Secrets → Actions → New secret**
```
SA_PASSWORD
```

This password is used for:
- SQL Server container authentication
- DbUp migration connection string

---

## 🧩 EF Core Usage in This Repo
EF Core is part of the application model but **not** used for schema deployment here.

You can use EF tooling for model info:
```bash
dotnet ef dbcontext info --project src/EfMigrations.Example
```

The database schema is controlled exclusively by SQL migration scripts executed by DbUp.

---

## 🔄 Using Grate or Flyway (Optional)
If you want to validate or migrate using additional tools, the SQL migration folder is compatible.

### Grate
Grate uses folder conventions (`up`, `down`, etc.). To use it:

1. Create folders, e.g.:
```
/grate.Example/up
```
2. Copy/symlink your existing scripts into `up/`
3. Run:
```bash
grate --connectionstring "Server=localhost,1433;Database=EfMigrationExampleDb;User Id=sa;Password=Your_password123!;TrustServerCertificate=True"       --files "database/grate" --noninteractive
```

### Flyway
Flyway uses `V#__Description.sql` naming.

1. Rename files and place them into:
```
/database/flyway
```
2. Run:
```bash
flyway -url="jdbc:sqlserver://localhost:1433;databaseName=EfMigrationExampleDb;encrypt=false"        -user=sa -password=Your_password123!        -locations=filesystem:database/flyway migrate
```

You may run DbUp, Grate, or Flyway as:
- primary migrator
- validation tools
- environment‑specific tools (e.g., Grate for Dev, Flyway for Prod)

---

## 🏁 Summary
This repo demonstrates how to:

- Use **DbUp.Example** to apply SQL Server migrations
- Use EF Core safely without using EF migrations
- Validate all changes automatically in GitHub Actions
- Optionally integrate **Grate** or **Flyway** with minimal restructuring

This template provides a robust starting point for building a reliable, automated database‑migration workflow in your .NET applications.
