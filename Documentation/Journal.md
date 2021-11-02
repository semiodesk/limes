# Journal

## 2021
### Tuesday, 2nd November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Download ProfilesRNS from http://profiles.catalyst.harvard.edu/
- Read installation manual
  - Windows host environment required to run software
- Setup Docker environment on Windows host
  - Creating docker-compose.yml
  - Add [ASP.NET](https://hub.docker.com/_/microsoft-dotnet-framework-aspnet) 4.8 virtual machine
  - Add [Microsoft SQL Server](https://hub.docker.com/_/microsoft-mssql-server) 2019 virtual machine
- Install [Azure Data Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) and setup ProfilesRMS database
  - Execute ProfilesRNS_CreateDatabase.sql
  - Execute ProfilesRNS_CreateSchema.sql
  - Execute ProfilesRNS_CreateAccount.sql (modified password)
  - Copy files from Database\Data\* into db server directory /var/opt/mssql/data/ProfilesRNS/