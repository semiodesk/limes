USE [%database%];
GO

-- truncate the table first
TRUNCATE TABLE [Profile.Import].[PersonAffiliation];
GO
 
-- import the file
BULK INSERT [Profile.Import].[PersonAffiliation]
FROM '%affiliationFile%'
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
