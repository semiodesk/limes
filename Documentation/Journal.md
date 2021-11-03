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
- Install [Microsoft SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) (SSMS) and setup ProfilesRMS database
  - Execute ProfilesRNS_CreateDatabase.sql
  - Execute ProfilesRNS_CreateSchema.sql
  - Execute ProfilesRNS_CreateAccount.sql (modified password)
  - Copy files from Database\Data\* into db server directory /var/opt/mssql/data/ProfilesRNS/
- Project kick-off meeting with Antonella Succurro

### Tuesday, 3rd November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Switching from Docker-based SQL Server 2019 to locally installed Developer Edition
  - Reason: The system requires the use of _Integration Services_ which are not available in the Docker images
  - Enable Integration Services in setup
  - Enable Full Text Search in setup
- Ensure to run SSMS as Administrator to be able to connect with Integration Services (Install Guide p. 12)
- Ensure that the SQL Server Agent service is started before proceeding
- Modifying SQL job scripts
  - Replacing $(YourProfilesServerName) with "localhost" (including quotes) as the server runs on the development machine.
  - Replacing $(YourProfilesDatabaseName) with "ProfilesRNS" (including quotes)
- After success, 4 new jobs are listed in localhost -> SQL Server Agent -> Jobs
- Created temporary Google Maps API key for geocoding billed to Semiodesk
- When setting up the nighly job, ensure that the SQL command is correct (not as displayed in the manual):
  - EXEC [Framework.].[RunJobGroup] @JobGroup = 4