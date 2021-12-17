USE [%database%];
GO

EXEC [Profile.Import].LoadProfilesData @use_internalusername_as_pkey = 1
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 7
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 3
GO