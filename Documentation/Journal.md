# Journal

## 2021
### Tuesday, 2nd November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

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

### Wednesday, 3rd November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

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

### Thursday, 4rd November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigating issue #6: Invalid default route when navigating to the site
  - Solution: Set basePath in [Framework.].[Parameters] to a URL (i.e. http://localhost:55956) without trailing slash **and** clear all browser caches.
- Investigating issue #7: Regular JavaScript 'unkown' alert windows
  - Cause: The JavaScript code in the frontend calls the backend with invalid parameters in regular intervals
  - An invalid parameter for 'referenceActivityId' caused the backend to return HTTP 500 Internal Server Error
  - The result of the invalid request 'unknown' was displayed in a alert box very 30s
  - Solution: Added sanity checks to the JavaScript code to not call the backend with invalid parameters
- Analyzing the source code structure and features
- Manually added admin user through the database

### Friday, 5th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

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

### Monday, 8th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Investigate why the <code>PubMedDisambiguation_GetPubs</code> job that loads the publications for the persons in the database is failing with a HTTP status code of <code>-2</code>
  - Researching and debugging almost took a whole day without success..
  - Feedback from Nicholas Brown suggested to remove the <code>ProfilesRNS_CallPRNSWebservice.dtsx</code> package from SSIS and re-install using SSDT.
  - Installing the same package using the <code>Import_SSIS_packages.bat</code> script resolved the issue.
  - Nicholas suggested, however, to prefer installing using SSDT because other methods seem to fail often with SQL Server updates.
  - Useful article about installing SSDT extensions for VS 2019: https://www.mssqltips.com/sqlservertip/6481/install-sql-server-integration-services-in-visual-studio-2019/

### Tuesday, 9th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

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

### Wednesday, 10th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Arranged a call with Nicholas Brown today @ 5pm
- Did not manage to resolve the broken links issue yesterday, will talk about it with Nicholas
- Adapting page design to use the NGS-CN color scheme and make use of Bootstrap
- Call with Nicholas Brown @ 5pm
  - Nicholas suggested to use Divisions for picking the centers
  - Nicholas promised to send me some SQL scripts that will correct the errors in my db caused by a configuration mistake

### Thursday, 11th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Filled out application form for VPN account at Bonn University
- Executed scripts provided by from Nicholas Brown to fix the invalid search result links
- Adapting home page design to requirements provided by Antonella Succorro
- Further developing and adapting page designs of content pages

### Friday, 12th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Project meeting with Antonella Succorro @ 10am
- Discussed visual adaptations
- Antonella mentioned that the new logos are available in the shared drive in Corel Draw format
- Overall impression of design changes is good
- Continuing to improve visual styles and responsive layout

### Monday, 15th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Integrated new logos in the footer
- Finalizing visual adaptations and repsonsive layout
  - Site now fully responsive
  - Accessibility analysis with respect to Section 508 guidelines

### Tuesday, 16th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Jour fix meeting with Antonella Succorro @ 10am
  - Discussed state of VPN access for Sebastian
  - Discussed visual adaptations
  - Discussed data transformation and loading tasks
- Started work on data transformation scripts

### Wednesday, 17th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Working on data transformation scripts (csv-clean.sh, csv-convert.sh)
  - Adding command line argument parser and documentation to scripts
  - Improving cleaning CSV data
  - Improving converting CSV data to ProfilesRNS

### Thursday, 18th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Working on data transformation scripts (csv-merge.sh, csv-filter.sh)
  - Adding command line argument parser and documentation to scripts
  - Improving merging CSV files
  - Improving filtering CSV files

### Friday, 19th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Finalized work on data transformation scripts (csv-clean.sh, csv-keyphrases.sh)
  - Adding command line argument parser and documentation to scripts
  - Improved phone number cleansing in csv-clean
  - Improved extraction of keyphrases
- Sent preliminary data to Antonella Succorro for review

### Monday, 22th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Analyzing preliminary data for errors
  - Wrong phone number for huettel@mpipz.mpg.de: Missing German prefix for Cologne
  - Double entry for wolfram.kunz@ukbonn.de with different email addresses
  - Three records are empty because they filled out a reduced form without a record in the original data
  	- mnothnag@uni-koeln.de
  	- holger.schwender@hhu.de
  	- sherms@uni-bonn.de
  - Inconnsitent spelling of street addresses 'Universitätsstr.', 'Universitaetsstraße', 'Universitätsstrasse'..
  - City missing for many records where there is a ZIP code
  - Some affiliation string are missing the %% quoting and will not be included in the key phrases
  
### Tuesday, 23th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Jour Fix meeting with Antonella Succorro @ 10am
  - Discussed issues with the data
   - Explained the refacored Python scripts
   - Data looks good overall
   - huettel@mpipz.mpg.de has a wrong email address. Another dataset exists for him.
   - Antonella will check the other two records
  - Discussed missing VPN access
  - Discussed missing SSL key
  - Discussed missing Google Maps key
  - Discussed next steps and project timeline
   - If everything goes well we could go live next week
   - However, there will be no time for testing, error corrections or other issues
   - Will use the extended period to get a polished and production ready service
- Adding generic find/replace functionality to 'csv-clean.py'
   
### Wednesday, 24th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Refactored text replacement from text file into Python script
- Refining ETL process
  - Adding additional cleansing steps
- Fixing issues in the data: changed e-mail address for 1 person

### Thursday, 25th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Implementing semantic data checks for phone numbers and addresses
- Fixing issues in the data: wrong addresses and phone number prefixes
- Integrating new data from Antonella Succorro

### Friday, 26th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Loading data into dev instance
- Added footer logo links and alt text
- Improving site navigation and responisve layout
- Improving accessibility
  - Adapting color scheme to match AA standards for contrasts and color-blindness
- Added imprint and privacy policy text

### Monday, 29th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Loading data into dev instance again
  - Windows shut down my last attempt on Friday during the night because of an Update
  - Process currently takes longer than 4 hours in the SNA.Coauthor.UpdateBetweeness job
- Improving accessibility of the site
  - Adding aria labels
  - Fixed form label references
  - Fixed small fontsizes
  - Removed noscript elements
  - Drop down menus are now opening on click instead of hover to make them usable on mobile phones and narrators
  - Improved focus and active state stylesheets for links and buttons
  - Improved taborder
- Improved Imprint layout

### Tuesday, 30th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Debugging issue #19: Broken advanced search for people and publications
  - Form data does not get parsed correctly in the code-behind
  - Reason is that there is only a single HTML form element
    - Moving the search input masks to a single page broke the initializiation of this form.
    - There is a **single** huge search parameter parsing function in the code behind for all of the search boxes. Bad practise.
    - Checking if the order of the search boxes makes a difference..
  - Refactored search forms into separate forms which directly submit their data to the corresponding search handler
  - Added form validation and disabled state for the submit button
  - Testing of the search functionality
    - Find people by keyword (quicksearch)
    - Find people by attributes
    - Find publication by keyword
  - Improved list pagination appearance, accessibility and handling
  - Found bug in search results for everything:
    - People cannot be navigated to.
    - Bug also present in stable version.

### Wednesday, 1st December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- My daughter opened the first door on the advent calendar.. what a happy day!
- Fixed #22: Cannot navigate search result items on 'everything' search result page
  - I did not want to fix this initially, however I figured that this bug also prevents people from navigating to publications, concepts etc.
  - Also when fixing this bug the search bar on top of the page could serve the everything search results, rendering the search box belwo the people search unnecessary
  - Fixed the bug and the JavaScript style overrides which had a bad impact on CSS styling and accessibility
- Fixed remaining accessiblity issues on frontpage
- Fixed #20: Removing addresses from contacts that do not want their address to be published
  - The address is not being written into the CSV export for ProfilesRNS
  - This way we do not need to adapt any code and there is no chance of data leaks
- Fixed #9: Invalid SSL certificate on production machine
  - Generating SSL cert using Letsencrypt
  - Followed this guide: https://www.snel.com/support/how-to-install-lets-encrypt-with-iis-on-windows-server-2019/

### Thursday, 2nd December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Preparing Go-Live of the website
- Cleaning up webserver
  - Installing Microsoft SSMS V18
  - Installing GitHub and checking out current repository
  - Setting up new website in IIS parallel to the old one so roll back is possible.
- Installing new site
  - Uploading new dataset
  - Installing new instance of Profiles RNS
  - Ingesting new data
  - Process takes 6,5h to complete
  - Website is broken, does not display data correctly
  - Rolling back to old site
  - Wrote email to Nicholas Brown to ask for help

### Friday, 3rd December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)

- Re-installing the site
  - Re-Uploading the data
  - Probably made a mistake with the installation SQL scripts
  - VM memory low
  - Data loading process takes about 7h
  - Site is now operational
  - Nicholas Brown confirmed this was the correct action to take

### Monday, 6th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Did not work in the morning because of doctor appointment
- Updated cookie section of Privacy Policy
  - Mentioning Profiles RNS instead of Wordpress
  - Added section about session cookies
- Fixed #29: Missing competence center dropdown on frontpage
- Fixed #24: JavaScript popup windows
  - Writing errors to the dev console instead of an alert window
- Fixed #27: Missing cookie consent banner
  - Implemented custom banner based on https://www.jqueryscript.net/other/cookie-consent-banner-localstroage.html
  - MIT License
- Fixed #25: Added URL Redirect from profiles-wggc.uni-bonn.de to profiles-ngs-cn.uni-bonn.de
- Fixed #28: Added HTTPS Upgrade to profiles-ngs-cn.uni-bonn.de in ISS Manager
- Renewed SSL certificates and added profiles-ngs-cn.uni-bonn.de (without www. prefix)
  - Used this tool: https://github.com/aloopkin/WinCertes
  - Command: ```wincertes -e contact.ccu@uni-bonn.de -d www.profiles-ngs-cn.uni-bonn.de -d profiles-ngs-cn.uni-bonn.de -w=c:\inetpub\wwwroot\www.profiles-ngs-cn.uni-bonn.de -b "NGS-CN Profiles" -p```

### Tuesday, 7th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Started work on the ETL and site update process
- After discussing the issue with Nicholas Brown I decided to develop an website data update tool
  - Reason: The data update process is mostly manual using a sequence of SQL queries
  - There is considerable room for error here which might render the site defective very easily
  - To enable non-developers to safely execute the update procedure, the tool should validate the data and execute the SQL queries in the correct order
- Since the server is a Windows machine and we're using SQL server, I decided to use .NET / C# as a programming language
  - Has the best bindings to the SQL Server and integration into the OS

### Wednesday, 8th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Continued work on Update Tool
  - Working Prototype


### Thursday, 9th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Jour Fix meeting with Antonella Succorro @ 11:30am
  - Discussed Google Maps keys / Geocoding keys
  - Did not manage to fix the Google Maps integration; postboned to following week
  - Discussed Update tool
  - Antonella said that the tools needs to be able to do partial updates
    - She does not always want to upload the entire 60+ CSV sheet to do simple updates like changing typos
    - Tool needs to be adapted to support this feature (load data from the server and merge it)
  - We need to add an additional column so that records can be marked as deleted
    - Tool will delete these records from the live instance
  - I said that the tool should be able to do updates in a non-production db
    - Meaning that it creates a new database and executes the database jobs on a database which is not served
    - Primary for the reason that the site always serves a consistent dataset
    - Secondary to have a backup / fallback strategy in case something goes wrong with the import

### Friday, 10th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Adapting the tool to meet the new requirements
- Partial update working
- Need to support match the column headers from the import file to the column headers of the db
  - These may be in the wrong oder and thus introduce errors
  - Will do this on Monday
- Will not implement the non-production db support as this turns out to be very complicated
  - Working on the data requires DB jobs to be installed that have a hard-coded link to a db
  - In order to execute these jobs on a temporary db I would have to install an manage the db jobs as well
  - This is out of scope for the remaining time we have

### Monday, 13th December
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Restarted the server in order to apply updates
  - However, some update could not be installed
  - Server needs to be updated by the IT department
- Continuing work on the update tool
  - Add support for deleting rows
  - Added support for matching column order
  - More testing and bugfixing
