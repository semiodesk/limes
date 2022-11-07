USE [%database%];
GO

-- It seems that the Profiles RNS import routine does not truncate this table
-- which can lead to unique key constraint errors.
-- See: https://github.com/semiodesk/limes/issues/38
TRUNCATE TABLE [Profile.Data].[Person.FacultyRank];
GO

EXEC [Profile.Import].LoadProfilesData @use_internalusername_as_pkey = 1
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 7
GO

EXEC [Framework.].[RunJobGroup] @JobGroup = 3
GO