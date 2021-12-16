# Server Maintenance Manual 1.0
Thursday, 16th December 2021, by [Sebastian Faubel](mailto:sebastian@semiodesk.com)

## 1. Introduction
This server is running [Profiles RNS](https://profiles.catalyst.harvard.edu/) research networking software, originally developed by [Harvard Catalyst](http://catalyst.harvard.edu/). The software is written in Microsoft .NET and heavily depends on Microsoft SQL server as a database.

In November 2021 the [Life and Medical Sciences (LIMES) Institute of Bonn University](https://www.limes-institut-bonn.de/startseite/) and [Semiodesk GmbH](https://semiodesk.com) cooperated in a project to develop an instance of Profiles RNS as a platform for [Next Generation Sequencing Competence Network](https://ngs-kn.de/). The following requirements were set at the beginning of the project:

1. Install a working instance of [ProfilesRNS 3.1](https://profiles.catalyst.harvard.edu/?pg=download&version=3.1.0) to be published on [www.profiles-ngs-cn.uni-bonn.de](https://www.profiles-ngs-cn.uni-bonn.de)
1. Visually adapt the system to complement the [official NGS-CN site](https://ngs-kn.de) corporate idendity
1. Add support for browsing the experts by competence center (CCGA, NCCT, WGGC)
1. Fix issues with special character support such as German Umlauts
1. Make the website GDPR compliant
1. Provide documentation, scripts and step-by-step instructions for site maintenance

Semiodesk formulated these additional goals:
1. Make the website responsive and mobile friendly
    - Improve useability of the site for mobile devices
    - Improve ranking in search engines
1. Make the website accessible and Section 508 compliant where possible

## 2. Installation and Setup
There is a detailed [Installation Guide](../src/ProfilesRNS/Documentation/ProfilesRNS_InstallGuide.pdf) provided by Profiles RNS. Follow these instructions as closely as possible as there are many details that matter. In addition to the information in the manual, please keep the following things in mind:

1. ℹ️ __Install the exact software versions__ that are described in the manual. The software makes use of many special features which might otherwise not be available.
1. ℹ️ **Set the ```basePath``` Parameter to the actual full website URL** where the site will be hosted. Excluding the trailing slash.

### 2.1 Folders
|Name|Folder|
|-|-|
|Website Release|```C:\inetpub\wwwroot\www.profiles-ngs-cn.uni-bonn.de```|
|Website Source Code|```C:\Users\Administrator\Documents\GitHub\limes\src\ProfilesRNS```|
|Database Files|```C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA```|
|Profiles RNS Manager Tool|```C:\Program Files\ProfilesRNS Manager```|
|Update Datasets|```C:\Users\Administrator\Desktop\ProfilesRNS```|

These folders should be backed up on a regular basis in order to prevent data loss.

## 3. Maintenance
The following tasks need to be done on a __regular basis__:
- Ensure the operating system software is up-to-date
- Change Administrator password on a regular basis
- Ensure all relevant folders are backed up
- Check if system resources are sufficient
    1. Open the start menu
    1. Type 'Task Manager'
    1. Click on the Task Manager app
    1. Check Memory Allocation (Should be ~60%)
    1. Check Storage Capacity on drive C (Should be ~60%)
- Check if all database jobs run correctly
    1. Open SQL Server Management Studio
    1. Connect to the local database using the Windows authentication
    1. Expand the tree to LIMES-GI0001 / SQL Server Agent
    1. Double-click in Job Activity Monitor
    1. Check if all scheduled jobs run sucessfully
- Check if the SSL certificates are valid
    1. Open the [website](https://profiles-ngs-cn.uni-bonn.de)
    1. If there is a browser certificate warning, renew the SSL certificates.
    1. See section 3.1 for details

### 3.1 Rewnewing the SSL certificates
The SSL certificates on this server are provided by [Let's Encrypt](https://letsencrypt.org) and managed on Windows using the Open Source software [WinCertes](https://github.com/aloopkin/WinCertes). Please refer to the offical website for detailed documentation about the tool.

With the following Windows terminal command you can generate SSL certificates for the production site and install a scheuled task for automatic renewal:

```
wincertes -e contact.ccu@uni-bonn.de -d www.profiles-ngs-cn.uni-bonn.de -d profiles-ngs-cn.uni-bonn.de -w=c:\inetpub\wwwroot\www.profiles-ngs-cn.uni-bonn.de -b "NGS-CN Profiles" -p
```

You can check the automated renewal tasks using the Windows built-in **Task Scheduler**:

<img src="Images/Screenshot Task Scheduler.png">

## Updating Profiles RNS
Updating Profiles RNS to newer software version is not a simple task and cannot be performed without ASP.NET development experience. This is due to the fact that the visual templates and the business logic code in the software is heavily interweaved.

Because we applied visual adaptations as well as bugfixes to the software, upgrading to a newer version requires a merge of the existing code base with the new version. We suggest to do this on a development machine with the production data to test the merged codebase before promoting it to production.

The website source code can be found in the folder described in [chapter 2.1](#2-1). Information about how to publish the code base to prodction can be found in [chapter 5](#5).

## 4. Updating Profile Data
- Create CSV datasets using the Python tools
    - csv-clean.py
    - csv-merge.py
    - csv-filter.py
    - csv-convert.py
    - csv-keyphrases.py
    - processdata.sh
- Update data using the Profiles RNS Manager

## 5. Changing Static Website Content
- Open Visual Studio
- Edit .aspx page
- Compile a release version
- Use the tree sync software to publish