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

### Wednesday, 3rd November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

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

### Thursday, 4rd November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigating issue #6: Invalid default route when navigating to the site
  - Solution: Set basePath in [Framework.].[Parameters] to a URL (i.e. http://localhost:55956) without trailing slash **and** clear all browser caches.
- Investigating issue #7: Regular JavaScript 'unkown' alert windows
  - Cause: The JavaScript code in the frontend calls the backend with invalid parameters in regular intervals
  - An invalid parameter for 'referenceActivityId' caused the backend to return HTTP 500 Internal Server Error
  - The result of the invalid request 'unknown' was displayed in a alert box very 30s
  - Solution: Added sanity checks to the JavaScript code to not call the backend with invalid parameters
- Analyzing the source code structure and features
- Manually added admin user through the database

### Friday, 5th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Analyzing the structure of the source code
- Meeting prepations
- Project meeting with Antonella Succurro @ 11am
  - Production environment specs
    - Virtual machine in computing center of Bonn University
    - Windows Server 2019
    - Memory: 8GB
    - Storage: 160GB
  - Production environment access
    - Endpoint and Credentials shared
    - Access to the machine via RDP did not work
    - Anotnella wants to investigate
  - Access to the existing production data
    - Antonella shared access to the GitLab account where the data is hosted
  - Authentication mechanism for production
    - Single administrator user; no other user profiles
    - Investigate disabling the login from the web
  - Google Maps API key
    - Will be provided by Antonella
  - Requirements specification for requested system modifications
    - Will be topic of the next jour fix meeting on Nov 9th
  - Production system:
    - https://www.profiles-wggc.uni-bonn.de/profiles/search/
    - SSL Certificate expired
- Importing the prodided production data using SSMS
  - In the Import Data dialog does not work with the provided queries because the data is in UTF-8 encoding and some columns require LATIN-1
  - Created SQL queries for importing the data

### Monday, 8th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigate why the <code>PubMedDisambiguation_GetPubs</code> job that loads the publications for the persons in the database is failing with a HTTP status code of <code>-2</code>
  - Researching and debugging almost took a whole day without success..
  - Feedback from Nicholas Brown suggested to remove the <code>ProfilesRNS_CallPRNSWebservice.dtsx</code> package from SSIS and re-install using SSDT.
  - Installing the same package using the <code>Import_SSIS_packages.bat</code> script resolved the issue.
  - Nicholas suggested, however, to prefer installing using SSDT because other methods seem to fail often with SQL Server updates.
  - Useful article about installing SSDT extensions for VS 2019: https://www.mssqltips.com/sqlservertip/6481/install-sql-server-integration-services-in-visual-studio-2019/

### Tuesday, 9th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigate why the <code>PubMedDisambiguation_GetPubsMEDXML</code> job that loads the publications does not retrieve any results
- Jour Fix meeting with Antonella Succorro @ 11am
  - Project Status
    I am currently having issues with loading the publication data. The SQL scripts that load publications from the Harvard site do not work reliably. I want to set up a call with Nick to resolve this issue as soon as possible.
  - Visual Adaptations
    We discussed that the appearance of the site should be based on the colors from this site: https://ngs-kn.de/ . We talked about the four logos of the partner organizations to be put in the header of the site. I will create an initial proposal for the adapted site layout and we will discuss it in the meeting.
  - Data Adaptations
    1. Remove the faculty drop down from the search form and replace it with a list of the partner organizations. This can either be achieved by introducing a new ontology term or by utilizing the group feature of the software. I will this discuss this with Nick.
    1. Adapt the data loading so that the ORCID ids of persons are processed and displayed on the site.
    1. Adapt the data presentation so that the address information can be hidden for some persons who do not approve the publication of their data on the site.

  - Next steps:
    - I will set up a call with Nick and discuss the data loading issues.
    - I will fill out the application form to get access to the production machine via VPN.
    - Antonella and Miriam want to provide the logos and a prototype for the site footer.
    - Antonella wants to take care of the outdated SSL certificate [#9](https://github.com/semiodesk/limes/issues/9) and the Google Maps API [#4](https://github.com/semiodesk/limes/issues/4) key.
- Fixing broken links on search results page [Github issue #15](https://github.com/semiodesk/limes/issues/15)

### Wednesday, 10th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Arranged a call with Nicholas Brown today @ 5pm
- Did not manage to resolve the broken links issue yesterday, will talk about it with Nicholas
- Adapting page design to use the NGS-CN color scheme and make use of Bootstrap
- Call with Nicholas Brown @ 5pm
  - Nicholas suggested to use Divisions for picking the centers
  - Nicholas promised to send me some SQL scripts that will correct the errors in my db caused by a configuration mistake

### Thursday, 11th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Filled out application form for VPN account at Bonn University
- Executed scripts provided by from Nicholas Brown to fix the invalid search result links
- Adapting home page design to requirements provided by Antonella Succorro
- Further developing and adapting page designs of content pages

### Friday, 12th November [Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Project meeting with Antonella Succorro @ 10am
- Discussed visual adaptations
- Antonella mentioned that the new logos are available in the shared drive in Corel Draw format
- Overall impression of design changes is good
- Continuing to improve visual styles and responsive layout