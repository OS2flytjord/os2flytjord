GO
ALTER TABLE [dbo].[Anmeldelse]
ADD FlagJordForurening uniqueidentifier null

-- =============================================
-- Author:		AJE
-- Create date: 19/01 2016
-- Description:	SP TO GET ALL Anmeldelser WHERE Status IS "Afsendt"!
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_SELECT_Anmeldelser]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT O.Id, M.Ejerlav, M.Ejerlavsnavn, M.Matrikelnr, JKT.Navn, K.Id AS KommuneId, K.Kommunenr, O.Geom AS OGeom, M.Geom AS Mgeom, JFO.JordKlassifikationTypeId, O.OprindelsesstedKlassifikationTypeId FROM [Oprindelsessted] AS O
	INNER JOIN Jord AS J ON J.Id = O.Id
	INNER JOIN Jordforureningsopslag AS JFO ON JFO.Id = O.Id
	INNER JOIN Matrikel AS M ON M.OprindelsesstedId = O.Id
	INNER JOIN Anmeldelse AS A ON A.Id = O.Id
	INNER JOIN Kommune AS K ON K.Id = A.KommuneId
	INNER JOIN JordKlassifikationType AS JKT ON JKT.Id = J.JordKlassifikationTypeId
		WHERE O.Id IN (
			SELECT DISTINCT [AnmeldelseId] FROM [StatusAnmeldelse]			
			WHERE [AnmeldelseId] IN (
				SELECT [AnmeldelseId]      
				FROM [StatusAnmeldelse]
				GROUP BY [AnmeldelseId], StatusAnmeldelseTypeId
				HAVING StatusAnmeldelseTypeId = 'FE83DA7A-9593-4321-8291-20FC32A5E854'
			)
			/*
			WHERE [AnmeldelseId] NOT IN (
				SELECT [AnmeldelseId]      
				FROM [StatusAnmeldelse]
				GROUP BY [AnmeldelseId], StatusAnmeldelseTypeId
				HAVING StatusAnmeldelseTypeId = '59FCF1F5-55FB-4E6E-A111-A3F836086942'
			)
			*/
		)		
	--AND O.Id = 'C378160E-A88A-4F79-A16F-DDCC0B095167'	
END
GO

-- =============================================
-- Author:		AJE
-- Create date: 26-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_SELECT_KommuneJordklassifikation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT KJK.KommuneId, KJK.JordKlassifikationTypeId, JKT.Kode, JKT.Priotering
	FROM [KommuneJordklassifikation] AS KJK
	INNER JOIN JordKlassifikationType AS JKT ON JKT.Id = KJK.JordKlassifikationTypeId
	WHERE KJK.Aktiv = 1
END
GO

INSERT INTO [dbo].[StatusAnmeldelseType]
           ([Id]
           ,[Navn]
           ,[Aktiv]
           ,[Kode])
     VALUES
           ('75ad2778-2898-4eed-a27b-390a8acda49c'
           ,'Forureningsstatus i miljø db er ændret'
           ,1
           ,14)
GO

-- =============================================
-- Author:		AJE
-- Create date: 27-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_UPDATE_Anmeldelse]
	@Id uniqueidentifier,
	@JFId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Anmeldelse]
	SET [FlagJordForurening] = @JFId
	WHERE [Id] = @Id

	DECLARE @StatusAnmeldelseId uniqueidentifier
	SET @StatusAnmeldelseId = null

	SELECT @StatusAnmeldelseId = Id FROM StatusAnmeldelse WHERE AnmeldelseId = @Id AND StatusAnmeldelseTypeId = '75ad2778-2898-4eed-a27b-390a8acda49c'

	IF(@StatusAnmeldelseId IS NULL)
	BEGIN
		INSERT INTO [dbo].[StatusAnmeldelse] (AnmeldelseId, StatusAnmeldelseTypeId, Tid) VALUES (@Id, '75ad2778-2898-4eed-a27b-390a8acda49c', GETDATE())
	END
END
GO

-- =============================================
-- Author:		AJE
-- Create date: 29-01-2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_BJ_UPDATE_ResetAnmeldelser]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE [dbo].[Anmeldelse]
	SET [FlagJordForurening] = NULL
	WHERE [FlagJordForurening] IS NOT NULL
END
GO