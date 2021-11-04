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
  - Replace $(YourProfilesServerName) with "localhost" (including quotes) as the server runs on the development machine.
  - Replace $(YourProfilesDatabaseName) with "ProfilesRNS" (including quotes)
- After success, 4 new jobs are listed in localhost -> SQL Server Agent -> Jobs
- Created temporary Google Maps API key for geocoding billed to Semiodesk
- When setting up the nighly job, ensure that the SQL command is correct (not as displayed in the manual):
  - EXEC [Framework.].[RunJobGroup] @JobGroup = 4
- When testing the application DB user account, [make sure mixed auth is enabled for the SQL server](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/change-server-authentication-mode?view=sql-server-ver15)
- Important: Set the "basePath" value in [Framework.].[Parameter] to the actual __**base URI**__. In a development environment this is 'http://localhost:55956'. Otherwise the site won't work.
- The parameter name is misleading as it suggests that this is only the path component of the base URI.

### Tuesday, 4rd November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigating issue #6: Invalid default route when navigating to the site
  - Solution: Set basePath in [Framework.].[Parameters] to a URL (i.e. http://localhost:55956) without trailing slash **and** clear all browser caches.
- Investigating issue #7: Regular JavaScript 'unkown' alert windows
  - Cause: The JavaScript code in the frontend calls the backend with invalid parameters in regular intervals
  - An invalid parameter for 'referenceActivityId' caused the backend to return HTTP 500 Internal Server Error
  - The result of the invalid request 'unknown' was displayed in a alert box very 30s
  - Solution: Added sanity checks to the JavaScript code to not call the backend with invalid parameters