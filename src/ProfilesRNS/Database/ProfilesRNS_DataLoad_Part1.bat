SET ContainerId=src-srv-sql-1
docker exec -d %ContainerId% mkdir /var/opt/mssql/data/ProfilesRNS/
docker cp Data\ORNG_1.0.owl %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\PRNS_1.4.owl %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\SemGroups.xml %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\SemTypes.xml %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\VIVO_1.4.owl %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\InstallData.xml %ContainerId%:/var/opt/mssql/data/ProfilesRNS/
docker cp Data\MeSH.xml %ContainerId%:/var/opt/mssql/data/ProfilesRNS/