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
  - Trying to harmonize department and instituation names
  - Setting up SPARQL endpoint in Fusike for graph analysis
  - All articles with names of authors are present in the database
  - Some co-authors are detected; trying to find differences.
  - No success :(

### Wednesday, 9th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Working on issue #36
  - Wrote e-mail to Nicholas Brown requesting help
    - He replied that this is a bug in Harvard's disambiguation engine:
    - It cannot cope with people having multiple affiliations
    - He created a fix for that and proposed it for integration
  - Resources with missing pulications in local development instance:
    - http://localhost:55956/profile/278102
    - http://localhost:55956/profile/39922
    - http://localhost:55956/profile/278100
    - http://localhost:55956/profile/39908
    - http://localhost:55956/profile/39930
    - http://localhost:55956/profile/39926
    - http://localhost:55956/profile/39927
  - Observations:
    - All resources without publications have a Closeness value of 0
 - Pages with errors:
   - http://localhost:55956/profile/278065


#### Queries
##### Force update of publications
```
EXEC msdb.dbo.sp_start_job 'PubMedDisambiguation_GetPubs'
GO

EXEC msdb.dbo.sp_start_job 'PubMedDisambiguation_GetPubMEDXML'
GO

EXEC msdb.dbo.sp_start_job 'ProfilesRNS_GetBibliometrics'
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 7
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 3
GO
```

##### All resources with num of pulications
```
SELECT
	t.[Subject],
	CONCAT('https://www.ProfilesRNS.uni-bonn.de/display/', t.[Subject]),
	p.[FirstName],
	p.[LastName],
	CONCAT(SUBSTRING(p.[FirstName],0,2), SUBSTRING(p.[LastName],0,2)) AS Initials,
	p.[InstitutionName],
	p.[DepartmentName],
	p.[NumPublications],
	p.[Closeness]
FROM
	[ProfilesRNS].[RDF.].[Node] n
JOIN
	[ProfilesRNS].[RDF.].[Triple] t ON t.[Object]=n.[NodeID]
JOIN
	[ProfilesRNS].[Profile.Cache].[Person] p ON p.[EmailAddr]=n.[Value]
WHERE
	p.NumPublications=0
ORDER BY
	p.InstitutionName, p.DepartmentName, p.NumPublications, p.Closeness
```

##### Describe resource
```
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>

SELECT ?s ?p ?o
WHERE
{
  ?s ?p ?o .
  FILTER(?s = <http://localhost:55956/profile/282083>)
}
```

##### All articles authored by "Beyer M"
```
PREFIX bibo: <http://purl.org/ontology/bibo/>
PREFIX vivo: <http://vivoweb.org/ontology/core#>
PREFIX prns: <http://profiles.catalyst.harvard.edu/ontology/prns#>
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>

SELECT ?year ?title ?authors ?url
WHERE
{
  ?url a bibo:Article ;
    rdfs:label ?title;
    prns:hasAuthorList ?authors;
    prns:year ?year .
    
  FILTER(CONTAINS(?authors, "Beyer M"))
}
ORDER BY DESC(?year)
LIMIT 100
```

##### Common relations between resources
```
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>

SELECT DISTINCT ?s ?type
WHERE
{
  ?s a ?type;
  {
    ?s ?p0 <http://localhost:55956/profile/39927>;
    ?p1 <http://localhost:55956/profile/39926> ;
    ?p2 <http://localhost:55956/profile/278100> ;
    ?p3 <http://localhost:55956/profile/39922> ;
    ?p4 <http://localhost:55956/profile/39908> ;
    ?p5 <http://localhost:55956/profile/39930> ;
    ?p6 <http://localhost:55956/profile/278102> ;
    ?p7 <http://localhost:55956/profile/39921> .
  }
  UNION
  {
  	<http://localhost:55956/profile/39927> ?p0 ?s.
    <http://localhost:55956/profile/39926> ?p1 ?s.
    <http://localhost:55956/profile/278100> ?p2 ?s.
    <http://localhost:55956/profile/39922> ?p3 ?s.
    <http://localhost:55956/profile/39908> ?p4 ?s.
    <http://localhost:55956/profile/39930> ?p5 ?s.
    <http://localhost:55956/profile/278102> ?p6 ?s.
    <http://localhost:55956/profile/39921> ?p7 ?s.
  }
}
```

##### Export RDF Graph to N-Triples
```
SELECT CAST(CONCAT('<', s.[Value], '>') AS NVARCHAR(1000)),
      CAST(CONCAT('<', p.[Value], '>') AS NVARCHAR(1000)),
      CAST(
        --- Some URI nodes are actually plain text nodes that start with a URI..
        CASE WHEN o.[ObjectType] = 0 AND NOT o.[Value] LIKE '% %' THEN
          CONCAT('<', o.[Value], '>')
        ELSE
          CONCAT('"', SUBSTRING(TRIM(REPLACE(REPLACE(REPLACE(o.[Value], CHAR(13), 'X'), CHAR(10), ' '),'"','')) ,0, 250), '"')
        END
        AS NVARCHAR(4000)
	  ),
	  '.'
FROM [ProfilesRNS].[RDF.].[Triple]
   JOIN [ProfilesRNS].[RDF.].[Node] s ON s.NodeID = [Subject]
   JOIN [ProfilesRNS].[RDF.].[Node] p ON p.NodeID = [Predicate]
   JOIN [ProfilesRNS].[RDF.].[Node] o ON o.NodeID = [Object]
```

After storing the query output as a file one can convert it into N-Triples with the following bash script:

```
#! /bin/bash

echo "Removing excess whitespace.."
sed 's/\s\s*/ /g' ProfilesRNS.rpt > ProfilesRNS.tmp

echo "Removing report header and footer.."
sed -i '2d' ProfilesRNS.tmp
head -n -5 ProfilesRNS.tmp > ProfilesRNS.nt

echo "Done."
rm ProfilesRNS.tmp
```

### Thursday, 10th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Jour Fix meeting with Antonella Succoro at 11am
  - Status of issue #36
    - Antonella will write e-mail to Nicholas about other imporvements to the disambiguation engine
  - Dicussing feedback regarding the frontpage design
    - Choosing the microscope icon
  - Server needs updates again
    - Antonella will upgrade VM on Friday
  - ~~Change default search type to 'everything'~~
  - ~~Change the label of default search~~
  - ~~Add Twitter to frontpage~~

### Friday, 11th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Adapting ActivityHistory SQL data source to include more details about publications
- Support call with Antonella Succoro
  - Investigating Twitter timeline not appearing on website
  - Cause: Ad-Blocker

### Monday, 14th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Adapting ActivityHistory control to display new data
- Jour Fix meeting with Antonella Succoro at 10:30am
  - Introduced Matomo
    - Scheuled log file parsing
    - Stats, Goals, User Engagement

### Tuesday, 15th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Adapting ActivityHistory control to display new data
- Moved ActivityHistory control to frontpage
- Altered page layout of frontpage
  
### Wednesday, 16th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Fixed issue #43
  - Caused by Profiles RNS storing the last query in a session variable and preferring this value over the query parameters of the URL
  - Made search parser ignore the session variable and prefer the HTML query parameters instead
  
### Thursday, 17th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Fixed issue #39
  - SVG logo needed reworking of the embedded PNG file and converting the font to a path
- Fixed issue #43
  - Caused by Profiles RNS storing the last query in a session variable and preferring this value over the query parameters of the URL
  - Added hidden input field with 'new' search parameter to force Profiles RNS to parse the search results
- Fixed issue #44
- Fixed issue #45
  - Caused by preferring the HTML query parameters over the search string session variable
  - Reverted old patches to fix #43 and added hidden input parameter to search form
  - This way the search form submit forces new parameters to be parsed while the page navigation can still use the cached search

### Friday, 17th November
[Sebastian Faubel](mailto:sebastian@semiodesk.com)
- Worked on issue #40
  - Managed to enable matching Umlauts by their base character (i.e. 'kohrer' matches 'köhrer')
  - Tried to enable matching of alternative spellings such as 'ae' instead of 'ä'
    - Best possible implmentation given the CONTAINSTABLE full text search method of SQL server was to rewrite alternative spellings to 'koe*' instead of 'koehrer*'
    - However, this descreases the precision of the 'everything' search significantly *AND*
    - It poses a problem with names that are not meant to be shortened such as 'Michael'
    - Concluded not to enable this option.
  - Tried to list alternative searches on the Search results page
    - However, the search results are rendered using XSLT which does not allow for interaction
    - Implementing the feature would require a significant refactoring of the search results component.
- Added preliminary word cloud to the site