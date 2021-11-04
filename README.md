# ProfilesRMS for LIMES

Welcome to the project repository for the Expert Finder System of the [Life and Medical Sciences (LIMES) Institute](https://www.limes-institut-bonn.de/startseite/) of Bonn University.

# Contacts
 - [Dr. Antonella Succurro](https://www.limes-institut-bonn.de/forschung/arbeitsgruppen/unit-2/abteilung-schultze/mitarbeiter/mitarbeiter/) (Product Owner, WGGC Officer)
 - [Prof. Joachim Schultze](https://www.limes-institut-bonn.de/forschung/arbeitsgruppen/unit-2/abteilung-schultze/mitarbeiter/mitarbeiter/) (Principal Stakeholder, WGGC Spokesperson)
 - [Dr. Mariam Sharaf](https://www.limes-institut-bonn.de/forschung/arbeitsgruppen/unit-2/abteilung-schultze/mitarbeiter/mitarbeiter/) (Principal Stakeholder, NGS-CN Officer)
 - [Dipl. Inf. (FH) Sebastian Faubel](https://www.linkedin.com/in/sebastianfaubel) (Developer)

# Getting Started
The setup of the ProfilesRNS system requires a running version of Microsoft SQL Server and IIS. We initially took some effort to create Docker images for Microsoft SQL server to simplify deployment and initial setup. However, ProfilesRNS makes use of some services which are not available in the containerized versions of the software. Thus deployment and setup automation is effectivly not possible.

To setup a development or production environment of the software, follow the instructions in the [ProfilesRNS Install Guide](src/ProfilesRNS/Documentation/ProfilesRNS_InstallGuide.pdf) __very closely__. The required software can be downloaded from here:

 - [Microsoft SQL Server](https://www.microsoft.com/de-de/sql-server/sql-server-downloads)
 - [Microsoft SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
 - [Visual Studio 2019](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

# References
 - https://github.com/ProfilesRNS/ProfilesRNS