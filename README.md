# ProfilesRMS for LIMES

Welcome to the project repository for the Expert Finder System of the Life and Medical Sciences (LIMES) Institute of Bonn University.

# Contacts
 - Dr. Antonella Succurro (Product Owner, WGGC Officer)
 - Prof. Joachim Schultze (Principal Stakeholder, WGGC Spokesperson)
 - Dr. Mariam Sharaf (Principal Stakeholder, NGS-CN Officer)
 - Dipl. Inf. (FH) Sebastian Faubel (Developer)

# Getting Started
The setup of the ProfilesRNS system requires a running version of Microsoft SQL Server and IIS. To simplify the installation of the SQL server we have created a custom Docker image that
bundles all the required extensions.

## Installing Microsoft SQL Server
Execute the following commands to install a Microsft SQL Server 2019 running in a Ubuntu virtual machine:

```
cd src/Docker
docker build -t semiodesk/mssql-fts:2019-ubuntu mssql-fts/
docker-compose up
```

We recommend installing [Azure Data Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) to manage the database server. Before installing the system we need to copy some files that are loaded in the setup into the container:

```
cd src/ProfilesRNS/Database/

# Set the %ContainerId% variable in the following script to the container id of your SQL server
ProfilesRNS_DataLoad_Part1.bat
```

Now follow the instructions in the [ProfilesRNS Install Guid](src/ProfilesRNS/Documentation/ProfilesRNS_InstallGuide.pdf)