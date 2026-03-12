-- =============================================
-- Author:		AJE
-- Create date: 23 Nov. 2015
-- Description:	Anmeldelse Søgning
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_Anmeldelse_Soegning]
	@KommuneId uniqueidentifier = null,
	@ModtagerAnlaegXmlList XML = null,
	@FoerDate datetime = null,
	@EfterDate datetime = null,
	@LoebeNr numeric(10,0) = null,
	@ModtagerId uniqueidentifier = null,
	@TransportoerId uniqueidentifier = null,
	@AnmelderId uniqueidentifier = null,
	@Forureningskategori uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@LoebeNr IS NULL)
	BEGIN
		SELECT	A.[Id], 
				[KommuneId],
				[TransportoerId], 
				[AnmelderId], 
				[ModtagerAnlaegId],
				[BetalerId], 
				[SagsbehandlerId],
				[RevisionAfAnmeldelse],
				[Nummer],
				[BemaerkningTilAnmeldelse],
				[BemaerkningTilKommune], 
				[BemaerkningTilJordmodtager],
				[AarsagAfvisning],
				[BemarkningInternKommune],
				[KoertJordAlarm], 
				[AnmelderSagsnummer], 
				[FlagSagsbehandler], 
				[TotalMaengdeJordIkkeFJ],				
				[FlagJordForurening]
		FROM [dbo].[Anmeldelse] AS A
		LEFT JOIN StatusAnmeldelse AS SA ON SA.AnmeldelseId = A.Id
		LEFT JOIN StatusAnmeldelseType AS SAT ON SAT.Id = SA.StatusAnmeldelseTypeId
		--LEFT JOIN Oprindelsessted AS OS ON OS.Id = A.Id
		LEFT JOIN Jord AS J ON J.Id = A.Id
		WHERE (1=(CASE WHEN @KommuneId IS NULL THEN 1 ELSE 0 END) OR [KommuneId] = @KommuneId)
		AND (1=(CASE WHEN @ModtagerAnlaegXmlList IS NULL THEN 1 ELSE 0 END) OR [ModtagerAnlaegId] IN (SELECT G.i.value('.', 'uniqueidentifier') FROM @ModtagerAnlaegXmlList.nodes('/guid') AS G(i)))
		AND (1=(CASE WHEN @EfterDate IS NULL THEN 1 ELSE 0 END) OR (SA.Tid >= @EfterDate AND SAT.Kode = 1))
		AND (1=(CASE WHEN @FoerDate IS NULL THEN 1 ELSE 0 END) OR (SA.Tid <= @FoerDate AND SAT.Kode = 1))
		AND (1=(CASE WHEN @ModtagerId IS NULL THEN 1 ELSE 0 END) OR (A.ModtagerAnlaegId = @ModtagerId))
		AND (1=(CASE WHEN @TransportoerId IS NULL THEN 1 ELSE 0 END) OR (A.TransportoerId = @TransportoerId))
		AND (1=(CASE WHEN @AnmelderId IS NULL THEN 1 ELSE 0 END) OR (A.AnmelderId = @AnmelderId))
		AND (1=(CASE WHEN @Forureningskategori IS NULL THEN 1 ELSE 0 END) OR J.JordKlassifikationTypeId = @Forureningskategori)
	END
	ELSE
	BEGIN
		-- SEACHING ONLY ON NUMBER - LøbeNr!
		SELECT	[Id], 
			[KommuneId],
			[TransportoerId], 
			[AnmelderId], 
			[ModtagerAnlaegId],
			[BetalerId], 
			[SagsbehandlerId],
			[RevisionAfAnmeldelse],
			[Nummer],
			[BemaerkningTilAnmeldelse],
			[BemaerkningTilKommune], 
			[BemaerkningTilJordmodtager],
			[AarsagAfvisning],
			[BemarkningInternKommune],
			[KoertJordAlarm], 
			[AnmelderSagsnummer], 
			[FlagSagsbehandler], 
			[TotalMaengdeJordIkkeFJ],				
				[FlagJordForurening]
		FROM [dbo].[Anmeldelse]
		WHERE (1=(CASE WHEN @KommuneId IS NULL THEN 1 ELSE 0 END) OR [KommuneId] = @KommuneId)
		AND (1=(CASE WHEN @ModtagerAnlaegXmlList IS NULL THEN 1 ELSE 0 END) OR [ModtagerAnlaegId] IN (SELECT G.i.value('.', 'uniqueidentifier') FROM @ModtagerAnlaegXmlList.nodes('/guid') AS G(i)))
		AND Nummer = @LoebeNr
	END	

END
GO

-- =============================================
-- Author:		AJE
-- Create date: 30-11-2015
-- Description:	Hent alle anmeldere som er aktive
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_AnmelderList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.[Id] FROM Anmelder AS A
	LEFT JOIN Person AS P ON P.[Id] = A.[Id]
	WHERE P.[Aktiv] = 1
END
GO

-- =============================================
-- Author:		AJE
-- Create date: 27-11-2015
-- Description:	Hent alle modtageanlæg som er aktive
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_ModtageAnlaegList]	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Id]
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
      ,[KontaktpersonNavn]
      ,[KontaktpersonTlf]
      ,[KontaktpersonEmail]
      ,[KommuneKode]
      ,[Bemaerkning]
      ,[AktivFra]
      ,[AktivTil]
      ,[Advis]
  FROM [dbo].[ModtagerAnlaeg]
  WHERE [Aktiv] = 1
END
GO

-- =============================================
-- Author:		AJE
-- Create date: 30-11-2015
-- Description:	Hent alle transportører som er aktive
-- =============================================
CREATE PROCEDURE [dbo].[SP_SELECT_TransportoerList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT T.[Id], T.[Aktiv] FROM Transportoer AS T
	LEFT JOIN Person AS P ON P.[Id] = T.[Id]
	WHERE T.[Aktiv] = 1
	AND P.[Aktiv] = 1
END
GO


