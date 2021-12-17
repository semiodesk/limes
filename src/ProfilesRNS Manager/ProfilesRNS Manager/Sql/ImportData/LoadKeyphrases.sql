USE [%database%];
GO

-- truncate the table first
TRUNCATE TABLE [Profile.Data].[Publication.PubMed.DisambiguationAffiliation];
GO
 
-- import the file
BULK INSERT [Profile.Data].[Publication.PubMed.DisambiguationAffiliation]
FROM '%keyphrasesFile%'
WITH
(
		CODEPAGE = '65001',
		ROWTERMINATOR = '\n',
		TABLOCK
)
GO
