  DECLARE @PersonId uniqueidentifier

  DECLARE @UserId int
  SET @UserId = 858	-- HUST AT FINDE BRUGERID!
  
  DECLARE @KommuneId uniqueidentifier
  SET @KommuneId = 'E939BD4F-7A7D-4DB3-BDAA-58F47279A6E6' -- HUSK AT FINDE RIGTIG KommuneId!

  SELECT @PersonId = Id FROM [dbo].[Person] WHERE [BrugerId] = @UserId
  
  UPDATE [dbo].[webpages_Membership] SET IsConfirmed = 1 WHERE UserId = @UserId

  INSERT INTO [dbo].[webpages_UsersInRoles] (UserId, RoleId)
  VALUES (@UserId, 2) --KommuneAdmin

  INSERT INTO [dbo].[webpages_UsersInRoles] (UserId, RoleId)
  VALUES (@UserId, 4) --Sagsbehandler
  
  INSERT INTO [dbo].[PersonKommune] ([PersonId], [KommuneId])
  VALUES (@PersonId, @KommuneId)