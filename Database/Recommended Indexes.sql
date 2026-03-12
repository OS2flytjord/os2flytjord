CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_1_Recommended] ON [dbo].[Anmeldelse] 
(
	[KommuneId] ASC,
	[Id] ASC
)
INCLUDE ( [TransportoerId],
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
[FlagSagsbehandler]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_2_Recommended] ON [dbo].[Anmeldelse] 
(
	[AnmelderId] ASC,
	[ModtagerAnlaegId] ASC,
	[Id] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_3_Recommended] ON [dbo].[Anmeldelse] 
(
	[AnmelderId] ASC,
	[Id] ASC,
	[TransportoerId] ASC
)
INCLUDE ( [ModtagerAnlaegId]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_4_Recommended] ON [dbo].[Anmeldelse] 
(
	[SagsbehandlerId] ASC,
	[Id] ASC
)
INCLUDE ( [KommuneId],
[TransportoerId],
[AnmelderId],
[ModtagerAnlaegId],
[BetalerId],
[RevisionAfAnmeldelse],
[Nummer],
[BemaerkningTilAnmeldelse],
[BemaerkningTilKommune],
[BemaerkningTilJordmodtager],
[AarsagAfvisning],
[BemarkningInternKommune],
[KoertJordAlarm],
[AnmelderSagsnummer],
[FlagSagsbehandler]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_5_Recommended] ON [dbo].[Anmeldelse] 
(
	[ModtagerAnlaegId] ASC,
	[Id] ASC
)
INCLUDE ( [KommuneId],
[TransportoerId],
[AnmelderId],
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
[FlagSagsbehandler]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [IDX_Person_1_Recommended] ON [dbo].[Person] 
(
	[Id] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_Person_2_Recommended] ON [dbo].[Person] 
(
	[BrugerId] ASC,
	[Id] ASC
)
INCLUDE ( [FirmaoplysningerId],
[Navn],
[Efternavn],
[Adresse],
[Postnummer],
[Postdistrikt],
[Email],
[Telefon],
[Mobiltelefon],
[Aktiv],
[GodkendtAfFirma],
[By],
[FrivilligeAdvis],
[KoertJordAlarm]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_1_Recommended] ON [dbo].[StatusAnmeldelse] 
(
	[StatusAnmeldelseTypeId] ASC,
	[Id] ASC,
	[AnmeldelseId] ASC
)
INCLUDE ( [PersonId],
[Tid]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_2_Recommended] ON [dbo].[StatusAnmeldelse] 
(
	[AnmeldelseId] ASC,
	[StatusAnmeldelseTypeId] ASC
)
INCLUDE ( [Id],
[PersonId],
[Tid]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [IDX_StatusAnmeldelse_3_Recommended] ON [dbo].[StatusAnmeldelse] 
(
	[AnmeldelseId] ASC,
	[Tid] ASC,
	[StatusAnmeldelseTypeId] ASC,
	[Id] ASC
)
INCLUDE ( [PersonId]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [IDX_Vognlaes_1_Recommended] ON [dbo].[Vognlaes] 
(
	[AnmeldelseId] ASC,
	[Id] ASC
)
INCLUDE ( [LastbilId],
[Dato],
[MaengdeTon],
[MaengdeAksler],
[Afvist],
[AfvistNote]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
