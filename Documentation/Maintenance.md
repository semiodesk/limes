# Server Maintenance Manual 1.0
Written by [Sebastian Faubel](mailto:sebastian@semiodesk.com) on Thursday, 16th December 2021

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
1. Ensure the operating system software is up-to-date
1. Change Administrator password on a regular basis
1. Ensure all relevant folders are backed up
1. Check if system resources are sufficient
    1. Open the start menu
    1. Type <code>Task Manager</code>
    1. Click on the Task Manager app
    1. Check Memory Allocation (Should be ~60%)
    1. Check Storage Capacity on drive C (Should be ~60%)
1. Check if all database jobs run correctly
    1. Open SQL Server Management Studio
    1. Connect to the local database using the Windows authentication
    1. Expand the tree to <code>LIMES-GI0001</code> / <code>SQL Server Agent</code>
    1. Double-click in Job Activity Monitor
    1. Check if all scheduled jobs run sucessfully
1. Check if the SSL certificates are valid
    1. Open the [website](https://profiles-ngs-cn.uni-bonn.de)
    1. If there is a browser certificate warning, renew the SSL certificates.
    1. See [section 3.1](#3-1) for details

### 3.1 Rewnewing the SSL certificates
The SSL certificates on this server are provided by [Let's Encrypt](https://letsencrypt.org) and managed on Windows using the Open Source software [WinCertes](https://github.com/aloopkin/WinCertes). Please refer to the offical website for detailed documentation about the tool.

With the following Windows terminal command you can generate SSL certificates for the production site and install a scheuled task for automatic renewal:

```
wincertes -e contact.ccu@uni-bonn.de -d www.profiles-ngs-cn.uni-bonn.de -d profiles-ngs-cn.uni-bonn.de -w=c:\inetpub\wwwroot\www.profiles-ngs-cn.uni-bonn.de -b "NGS-CN Profiles" -p
```

You can check the automated renewal tasks using the Windows built-in **Task Scheduler**:

<img src="Images/Screenshot Task Scheduler.png">

## Software Update
Updating Profiles RNS to newer software version is not a simple task and cannot be performed without ASP.NET development experience. This is due to the fact that the visual templates and the business logic code in the software is heavily interweaved.

Because we applied visual adaptations as well as bugfixes to the software, upgrading to a newer version requires a merge of the existing code base with the new version. We suggest to do this on a development machine with the production data to test the merged codebase before promoting it to production.

The website source code can be found in the folder described in [chapter 2.1](#2-1). Information about how to publish the code base to prodction can be found in [chapter 5](#5).

## Data Update Process
Profiles RNS dos not have a graphical userinterface for managing its data. There is the possibility for users of the portal to log-in and change their profile data directly, however, as per specification this method should not be used. The main reason is that it would require to execute data merging before adding or removing existing profiles and thus complicating the overall process. It was a requirement that all data should be updated by NGS-CN personell.

For these reasons updating the website data involves a [ETL process](https://www.ibm.com/topics/etl) based on [CSV files](https://en.wikipedia.org/wiki/Comma-separated_values). These files usually originate from a web form where participants can insert and update their data. In the process these files will be cleanded, transformed and loaded into the database for import. The following figure illustrates the update process:

<img src="Images/Import Process.svg">

## Data Transformation Scripts
There are five tools to handle specific tasks in the data transformation pipeline. The output of each tool can be the input of the next in order to produce a clean dataset. The tools are based on the original work of [Dr. Antonella Succorro](https://www.limes-institut-bonn.de/forschung/arbeitsgruppen/unit-2/abteilung-schultze/mitarbeiter/mitarbeiter/):

### csv-clean.py
With this tool you can clean CSV files before data transformations. The cleansing operations are implemented using a so called 'cleaner' script which allows to apply all kinds of checks, such as sanity checking for ZIP codes and city names. There is currently one cleaner script with the id 'profiles-rns'.

#### Example
```
python csv-clean.py -i file.csv -o cleaned.csv -k id -c profiles-rns
```
Clean a file using the 'profiles-rns' cleaner.

#### Parameters
|Argument|Type|Description|
|-|-|-|
|-i, --input| required | Input file in CSV format to be read.|
|-o, --output| required | Output file in CSV format to be written.|
|-k, --key| required | Column name to be used to identify records.|
|-c, --cleaner| required | ID of the data cleaner to be used.|

In order to support new input formats for this tool, a specializd cleaner script needs to be implemented that understands the column and data structure. To implement a new cleaner, just copy the <code>lib/profiles_data_cleaner.py</code> code, adapt it to your needs and register the new cleaner in the <code>lib/csv_data_cleaner_factory.py</code>.

### csv-merge.py
With this tool you can merge two CSV files into one. It allows for merging columns and providing default values for empty cells. The input files are merged in the order that the rows of the second file are merged onto the records of the first file given a primary key.

#### Example
```
python csv-merge.py -ia file-a.csv -ib file-b.csv -o merged.csv -k id -d age:42 -m email:e-mail
```
Merge two files using the 'id' column and set the default value for the 'age' column to 42. Also merge the values from the column 'e-mail' in the second file into the column 'email' in the first, overwriting all existing values.

#### Parameters
|Argument|Type|Description|
|-|-|-|
|-ia, --input-a| required | First input file in CSV format to be read.|
|-ib, --input-b| required | Second input file in CSV format to be read.|
|-o, --output| required | Output file in CSV format to be written.|
|-k, --key| required | Column name to be used to identify records.|
|-d, --default-value| optional | Default value to be used if a column is empty in the format column_name:value (i.e. --default-value age:42)|
|-m, --merge-column| optional | Names of two columns to be merged in the format column_a:column_b. The columns are merged in the specified order.|

### csv-filter.py
With this tool you can remove records from a CSV file given a list of ids as input. It can operate in two modes: a) either remove all the ids in the given list or b) only keep the records specified in the list. In addition, the tool can filter records whose column values do not meet certain criteria, such as empty values.

#### Example
```
python csv-filter.py -i file.csv -o filtered.csv -k id --f filtered-ids.txt -v first-name:
```
Remove the ids from 'filtered-ids.txt' and all rows that have an empty value for the column 'first-name'.

#### Parameters
|Argument|Type|Description|
|-|-|-|
|-i, --input| required | Input file in CSV format to be read.|
|-o, --output| required | Output file in CSV format to be written.|
|-k, --key| required | Column name to be used to identify records.|
|-f, --filterlist| optional | Text file contaning one id per line of the rows to be included in the output.|
|-e, --exclude| optional | Trigger the ids in the filter list to be excluded from the output.|
|-v, --value| optional | Column values to be filtered in the format column:value |

### csv-convert.py
Transform CSV files into a ProfilesRNS import dataset. The mapping and serialization is handled by custom writer classes which are defined in the file <code>lib/profiles_converter.py</code>.

#### Example
```
python csv-convert.py -i file.csv -o output
```
Generate a ProfilesRNS dataset from the given input file.

#### Parameters
|Argument|Type|Description|
|-|-|-|
|-i, --input| required | Input file in CSV format to be read.|
|-o, --output| required | Output **folder** for the generated CSV files..|

### csv-keyphrases.py
Extract keyphrases from a column of a CSV file and write it into ProfilesRNS format. The values in the columns are
expected to be in the format %keyphrase%. Duplicates are removed from the keyphrases and they are written in alphabetical order.

#### Example
```
python csv-keyphrases.py -i file.csv -o keyphrases.txt -k keyphrases
```
Extract keyphrases from the column 'keyphrases' and write them into the flat list keyphrases.txt.

#### Parameters
|Argument|Type|Description|
|-|-|-|
|-i, --input| required | Input file in CSV format to be read.|
|-o, --output| required | Output file in CSV format to be written.|
|-k, --key| required | Column name where to extract the keyphrases.|

### processdata.sh
A script for the Bash command line defining the ETL pipeline that operates on the various NGS-CN datasets. Executing this script 

## Data Ingestion Tool
The data produced by the <code>csv-convert.py</code> script can directly be imported into ProfilesRNS 3.1. However, this process involves executing several SQL queries directly on the database which is a error prone process, especially when done by inexperienced people. To avoid corrupting the production site we developed a dedicated graphical user interface to simplify this task.

With our tool 'Profiles RNS Manager' an inexperienced person can perform an update of the Profiles RNS database without using SQL or other developer tools. The input to the tool is a folder that contains a specific set of files:

- Person.csv
- PersonAffiliation.csv
- PersonFilterFlag.csv
- Keyphrases.txt

These files are produced by the <code>csv-convert.py</code> script and directly correspond to tables in the ProfilesRNS database. Please see the Profiles RNS Install Manual for details on this topic.

> ℹ️ The tool also supports merging partial datasets with the production data for quick and easy updates or removals of individual records.

### Import a Dataset
To update the ProfilesRNS database follow these steps:

1. Upload a ProfilesRNS dataset onto the server
2. Put the dataset into a folder named 'YYYY-MM-DD' in the data folder descriped in [chapter 2.1](#2-1)
3. Open the 'ProfilesRNS Manager' application
4. Select the previously created folder
5. If everything is good, press the 'Import' button

**TODO**: Add screenshots

### Selecting a Site
When the tool is being started for the first time, one must select a site configuration file. This file contains the database connection details of the target site. To select a site, follow these steps:

1. Press the menu button marked by three lines
2. Select 'Change site..'
3. Navigate to the webroot of your production website
4. Select the file 'Web.config'
5. Press 'OK'

**TODO**: Add screenshots

## Changing Static Website Content
- Open Visual Studio
- Edit .aspx page
- Compile a release version
- Use the tree sync software to publish