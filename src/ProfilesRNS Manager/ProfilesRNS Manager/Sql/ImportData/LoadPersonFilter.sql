USE [%database%];
GO

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[PersonFilterFlag];
GO
 
-- import the file
BULK INSERT [Profile.Import].[PersonFilterFlag]
FROM '%filterFile%'
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
