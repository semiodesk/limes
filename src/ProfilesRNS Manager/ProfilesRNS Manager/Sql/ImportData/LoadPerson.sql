USE [%database%];
GO

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[Person];
GO
 
-- import the file
BULK INSERT [Profile.Import].[Person]
FROM '%personFile%'
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
