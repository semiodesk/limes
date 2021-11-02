# Microsoft SQL Server 2019 FTS
https://schwabencode.com/blog/2019/10/27/MSSQL-Server-2017-Docker-Full-Text-Search

This directory contains a custom docker image of Microsoft SQL Server with support for full-text indices. To create the image, execute the following code in this directory:

```
docker build -t semiodesk/mssql-fts:2019-ubuntu .
```