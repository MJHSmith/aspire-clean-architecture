# Database migrations

## Generating initial schema

Run the following command to create the initial migration for the SQL Server database. 
This will generate the necessary migration files in the specified output directory.

```powershell
dotnet ef migrations add SqlServerInitialCreate --project src\Infrastructure --output-dir Database/Migrations --startup-project src\Web.Api
```


## Generating migrations

Run the following command to create a migration script from the current sate of the database
to the current code configuration.

```powershell
dotnet ef migrations add {UniqueMigrationName} --project src\Infrastructure --output-dir Database/Migrations --startup-project src\Web.Api
```


## Apply migrations

Migrations will be applied when application starts.
