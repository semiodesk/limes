USE [ProfilesRNS];
GO

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[Person];
GO
 
-- import the file
BULK INSERT [Profile.Import].[Person]
FROM 'C:\...\Person.csv'
WITH
(
		CODEPAGE = '65001',
        FIRSTROW=2,
		FIELDTERMINATOR = ';', 
		ROWTERMINATOR = '\n',
		TABLOCK
)
GO

-- fix errors in the data; see output of validation routine
UPDATE [Profile.Import].[Person] SET State = NULL;
UPDATE [Profile.Import].[Person] SET middlename = '' WHERE middlename IS NULL;

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[PersonAffiliation];
GO
 
-- import the file
BULK INSERT [Profile.Import].[PersonAffiliation]
FROM 'C:\...\PersonAffiliation.csv'
WITH
(
		CODEPAGE = '65001',
        FIRSTROW=2,
		FIELDTERMINATOR = ';', 
		ROWTERMINATOR = '\n',
		TABLOCK
)
GO

-- fix errors in the data; see install guide p. 18
UPDATE [Profile.Import].[PersonAffiliation] SET institutionabbreviation = institutionname WHERE institutionabbreviation IS NULL;

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[PersonFilterFlag];
GO
 
-- import the file
BULK INSERT [Profile.Import].[PersonFilterFlag]
FROM 'C:\...\PersonFilterFlag.csv'
WITH
(
		CODEPAGE = '65001',
        FIRSTROW=2,
		FIELDTERMINATOR = ';', 
		ROWTERMINATOR = '\n',
		TABLOCK
)
GO

-- fix errors in the data
UPDATE [Profile.Import].[PersonFilterFlag] SET personfilter = '';

-- Finally run the validation script
EXEC [Profile.Import].ValidateProfilesImportTables