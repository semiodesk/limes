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

https://matomo.org/matomo-on-premise/?menu

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

https://goaccess.io/

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

https://www.graylog.org

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

https://www.elastic.co/de/what-is/elk-stack

- Very powerful, enterprise grade.
- All components are open source.
- However, not a good fit for on-premise installation.
  - Requires three services (Elasticsearch, Logstash and Kibana) to run and service.
  - Manual setup of Kibana dashboard required.
- Overkill.

#### SmarterStats (No Option)
<img src="https://www.smartertools.com/img/views/stats/log_analytics/analytics_understand_traffic.png"  width="500">

https://www.smartertools.com/smarterstats/log-analysis

- No free plan available.
  
#### ApacheViewer (No Option)
<img src="https://www.apacheviewer.com/wp-content/uploads/2015/04/shotA-1024x692.png" width="500">

https://www.apacheviewer.com/

- No free plan available.
- Cloud based.

#### Sumo Logic (No Option)
<img src="https://assets-www.sumologic.com/assets/refresh-images/screenshots/infrastructure-monitoring.jpg" width="500">

https://www.sumologic.com/

- No free plan available.

#### Deep Log Analyzer (No Option)
<img src="https://logicalread.com/wp-content/uploads/2021/02/image-2.png" width="500">

https://logicalread.com/best-log-analyzer-tools-for-iis-web-servers/

- No free plan available.

#### SolarWinds Loggly (No Option)
<img src="https://logicalread.com/wp-content/uploads/2021/02/image.png" width="500">

https://www.loggly.com/

- No free plan available.