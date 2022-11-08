# Journal

## 2022
### Monday, 31st October
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- VPN connection to University of Bonn still working.
- Connection to ProfilesRNS webserver still working.
- Download ProfilesRNS from http://profiles.catalyst.harvard.edu/
- Setup SQL Server 2019 on Windows host
- Updated project Git repository
- Project kick-off meeting with Antonella Succurro at 10am
- Evaluating IIS Log file analytics software
  - Sources
    - https://stackoverflow.com/questions/48925690/log-analyzer-tools-for-iis-webserver-logs
    - https://serverfault.com/questions/13676/any-freeware-iis-log-analyzer
    - https://logicalread.com/best-log-analyzer-tools-for-iis-web-servers/
    - https://cllax.com/top-7-iis-log-analyzer-tools.html
    - https://arstechnica.com/civis/viewtopic.php?t=1162685
    - https://www.tek-tools.com/apm/best-free-log-analysis-tools
  - Requirements
    - Free
    - GDPR compliant
    - Log file analytics
  - Notes
    - Many of the Microsoft [LogParser](https://www.microsoft.com/en-us/download/details.aspx?displaylang=en&id=24659) based solutions are not supported anymore.
    - LogParser itself has no UI.
- Installed Matomo on the production web server with PHP and MariaDB.
- Setup daily task to import the most recent web server log file into Matomo.

#### Matomo (Best Option)
<img src="https://m-img.org/spai/w_1696+q_lossless+ret_img+to_webp/https://static.matomo.org/wp-content/uploads/2022/03/Dasboard-01-01-2048x1264.png" width="500">

- https://matomo.org/matomo-on-premise/?menu
- Not primarily a log file analyzer but a JS based statistics software.
  - But has log analytics support.
  - Log analytics included in all plans.
- "It’s for this reason that Matomo On-Premise is, and forever will remain free to download."
  - Sounds good.
- GDPR compliant, no tracking JS required.
- Sustainability:
  - Used at UN, EU, AI and NASA.
  - Seems to be well funded.
- Bonus:
  - Could be used with Wordpress sites of LIMES so we have the same UI for all sites.
- Install:
  - Can be run in IIS with PHP support added.
  - May require MySQL to run, no big issue.
  - Easy to maintain
    - To update, simply extract the latest version in the webroot of the stats site.
    - We can try to setup updating via Git.

#### GoAccess (Alternative Option)
<img src="https://goaccess.io/images/goaccess-bright.png?20220930205119" width="500">

- https://goaccess.io/
- Report generator engine with HTML output.
- GDPR compliant, no tracking JS required.
- Sustainability:
  - Open Source, MIT License
  - GitHub Repository since 2011 and has recent commits.
- Install:
  - Requires Cygwin on Windows to compile
    - Which means its *hard* to update and maintain.
    - Would be much easier on Linux
  - Setup OS task to generate static report files.
    - Another possible pitfall for maintenance.
  - Does NOT require any database
    - Awesome.

#### GrayLog (Alternative Option)
<img src="https://www.graylog.org/wp-content/uploads/2022/08/Dashboard-p-500.jpeg?w=500" width="500">

- https://www.graylog.org
- Log analyzer platform.
- GDPR compliant, no tracking JS required.
- Sustainability:
  - Open Source, [SSPL](https://github.com/Graylog2/graylog2-server/blob/master/LICENSE)
  - GitHub Repository since 2011 and has recent commits.
- Install:
  - Java based
  - Focused on Linux
  - [No installer or instructions for Windows](https://docs.graylog.org/docs/installing)
  - Hard to setup and maintain.

#### ELK Stack (Alternative Option)
<img src="https://www.tek-tools.com/wp-content/uploads/2020/05/elk-stack-iii.png.jpg" width="500">

- https://www.elastic.co/de/what-is/elk-stack
- Very powerful, enterprise grade.
- All components are open source.
- However, not a good fit for on-premise installation.
  - Requires three services (Elasticsearch, Logstash and Kibana) to run and service.
  - Manual setup of Kibana dashboard required.
- Overkill.

#### SmarterStats (No Option)
<img src="https://www.smartertools.com/img/views/stats/log_analytics/analytics_understand_traffic.png"  width="500">

- https://www.smartertools.com/smarterstats/log-analysis
- No free plan available.
  
#### ApacheViewer (No Option)
<img src="https://www.apacheviewer.com/wp-content/uploads/2015/04/shotA-1024x692.png" width="500">

- https://www.apacheviewer.com/
- No free plan available.

#### Sumo Logic (No Option)
<img src="https://assets-www.sumologic.com/assets/refresh-images/screenshots/infrastructure-monitoring.jpg" width="500">

- https://www.sumologic.com/
- No free plan available.

#### Deep Log Analyzer (No Option)
<img src="https://logicalread.com/wp-content/uploads/2021/02/image-2.png" width="500">

- https://logicalread.com/best-log-analyzer-tools-for-iis-web-servers/
- No free plan available.

#### SolarWinds Loggly (No Option)
<img src="https://logicalread.com/wp-content/uploads/2021/02/image.png" width="500">

- https://www.loggly.com/
- No free plan available.

### Wednesday, 2nd November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Fixed issues with Matomo on production machine
  - Exclude sensitve folders from web access through IIS
    - config, tmp, misc
  - Fixed missing geo location and user agent info
    - Command line arguments to match the IIS log file syntax
    - Adjusted folder access rights to allow for write into tmp and misc
    - Installed DB-IP geolocation database with city level precision
  - Adjusted MySQL max_allowed_packet for better performance
  - Adjusted regular import task to match new site
- Notes
  - We seem to be running out of memory with MySQL and SQL server running in paralell
- Created SVG template for iterating on the new frontpage design

### Thursday, 3nd November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Investigating Goals not working in Matomo
- Investigating slow PHP speed on the server
- Jour Fix meeting with Antonella Succurro at 10am
  - Talked about issue #40
    - Evaluated possible solutions
    - Agreed to alter the no results page to show similar result if there are no results at all
  - Talked about issue #39
    - Will add the logo to the new design on top of the site
  - Taked about issue #38
    - Antonella will try to upload a consolidated dataset
  - Antonella restarted the server to install important updates, fixing #31
  - Talked about the feasability to implement type-ahead search suggestions
    - Low priority
    - Will try to see if the database is fast enough

### Friday, 4th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Working on the new frontpage designs
- Investigating issue #38 where the import fails with unique key constraint errors
  - The import fails even when 'Merge with existing data' is not activated
  - Profiles RNS import routine does not seem to recompute the ```[Profile].[Data].[FacultyRank]``` table
    - Therefore, newly imported data with same facultyrankorder values collide with existing ones
    - Quick solution: Truncate the table before import
  - Need to adapt the import application Profiles RNS Manager
  - Discuss the issue on next Monday's Jour Fix meeting

### Monday, 7th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Investigating why scheduled Matamo task does not run properly
  - Reason: Task did not run properly because the batch script contained a relative command but was not executed in C:\inetpub\logs
  - Added directory to task execution context
- Investigating why ProfilesRNS does not run publications load task
  - SQL ServerAgent is not running, probably because of server restart for updates
  - Started publication import
  - Publications were fetched but with publications after August 2022 missing
- Jour Fix meeting with Antonella Succurro at 10am
  - Discuss data import
  - Discuss FacultyRank issue
    - Not really used. Can be dropped.
    - Agreed to alter the import program to clear the tables upon import. 
  - Bug: Publications are missing July - November
    - High importance, possiblity related to issue #36
  - Antonella said that migration of batch script to Python would be nice but has low priority
- Working on frontpage designs
  - Sent two variatons to Antonella who picked variation A
  - She said that the icons with the advanced search options need improvement
  - Explored different icon variantes and sent two alternatives to Antonella
- Working on issue #36
  - Most probably caused by empty %% keyphrase in keyphrases.txt at line 1
  - This very likely caused the import to ignore all other keyphrases which leaves no seeds for publication retrieval

### Tuesday, 8th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Working on issue #36
  - Setting up SQL query to retrieve all profiles with no pulications
  - Trying to improve keyphrases so that more publications can be found

```
SELECT
	CONCAT('https://www.profiles-ngs-cn.uni-bonn.de/display/', t.[Subject]),
	p.[FirstName],
	p.[LastName],
	p.[InstitutionName],
	p.[DepartmentName],
	p.[NumPublications]
FROM
	[profiles-ngs-cn].[RDF.].[Node] n
JOIN
	[profiles-ngs-cn].[RDF.].[Triple] t ON t.[Object]=n.[NodeID]
JOIN
	[profiles-ngs-cn].[Profile.Cache].[Person] p ON p.[EmailAddr]=n.[Value]
ORDER BY
	p.[NumPublications]
```