GO
ALTER TABLE [dbo].[ModtagerAnlaeg]
    ADD [OphaevAutoGodkendAnmKom] BIT CONSTRAINT [DF_ModtagerAnlaeg_OphAutoGodkendAnmKommune] DEFAULT ((0)) NOT NULL;


GO
-- =============================================
-- Author:		AJE
-- Create date: 27-11-2015
-- Description:	TO GET ID AND NAME FROM ACTIVE MODTAGEANLÆG!
-- =============================================
ALTER PROCEDURE [dbo].[SP_SELECT_ModtageAnlaegList]	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id]
      ,[JordmodtagerId]
      ,[JordanlaegTypeId]
      ,[JordKlassifikationTypeId]
      ,[Navn]
      ,[Adresse]
      ,[Postnummer]
      ,[PostDistrikt]
      ,[Ejerlav]
      ,[Matrikelnr]
      ,[www]
      ,[OffentligBemaerkning]
      ,[StikproeveFrekvens]
      ,[Geom]
      ,[AnvenderJF]
      ,[Aktiv]
      ,[AntalBaase]
      ,[AdvisLabBaas]
      ,[Affald]
      ,[Nummer]
      ,[AutoGodkend]
      ,[OphaevAutoGodkendAnmKom]
      ,[KontaktpersonNavn]
      ,[KontaktpersonTlf]
      ,[KontaktpersonEmail]
      ,[KommuneKode]
      ,[Bemaerkning]
      ,[AktivFra]
      ,[AktivTil]
      ,[Advis]	
  FROM [dbo].[ModtagerAnlaeg]
	--WHERE [Aktiv] = 1
END
GO
