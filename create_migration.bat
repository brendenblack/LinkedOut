@echo off
if [%~1]==[] (
    @echo You must provide a migration name
    goto :eof
)

echo Generating SQL Server migrations with name %1
SET DatabaseType=SqlServer
dotnet ef migrations add "%1" --project src\LinkedOut.Infrastructure --startup-project src\LinkedOut.Api --context SqlServerApplicationDbContext --output-dir Persistence\Migrations\SQLServer
echo Generating PostgreSQL migrations with name %1
SET DatabaseType=PostgreSQL
dotnet ef migrations add "%1" --project src\LinkedOut.Infrastructure --startup-project src\LinkedOut.Api --context PostgreSqlApplicationDbContext --output-dir Persistence\Migrations\PostgreSQL