CREATE TABLE [dbo].[Anmeldelse] (
    [Id]                         UNIQUEIDENTIFIER CONSTRAINT [DF_Anmeldelse_Id] DEFAULT (newid()) NOT NULL,
    [KommuneId]                  UNIQUEIDENTIFIER NULL,
    [TransportoerId]             UNIQUEIDENTIFIER NULL,
    [AnmelderId]                 UNIQUEIDENTIFIER NOT NULL,
    [ModtagerAnlaegId]           UNIQUEIDENTIFIER NULL,
    [BetalerId]                  UNIQUEIDENTIFIER NULL,
    [SagsbehandlerId]            UNIQUEIDENTIFIER NULL,
    [RevisionAfAnmeldelse]       UNIQUEIDENTIFIER NULL,
    [Nummer]                     NUMERIC (10)     IDENTITY (1000, 1) NOT NULL,
    [BemaerkningTilAnmeldelse]   VARCHAR (MAX)    NULL,
    [BemaerkningTilKommune]      VARCHAR (MAX)    NULL,
    [BemaerkningTilJordmodtager] VARCHAR (MAX)    NULL,
    [AarsagAfvisning]            VARCHAR (MAX)    NULL,
    [BemarkningInternKommune]    VARCHAR (MAX)    NULL,
    [KoertJordAlarm]             INT              NULL,
    [AnmelderSagsnummer]         NVARCHAR (40)    NULL,
    [FlagSagsbehandler]          DATETIME         NULL,
    [TotalMaengdeJordIkkeFJ]     INT              NULL,
    [FlagJordForurening]         UNIQUEIDENTIFIER NULL,
    [GebyrId]                    UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Anmeldelse] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Anmeldelse_Anmeldelse] FOREIGN KEY ([RevisionAfAnmeldelse]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [FK_Anmeldelse_Gebyr] FOREIGN KEY ([GebyrId]) REFERENCES [dbo].[Gebyr] ([Id]),
    CONSTRAINT [Jordtip_Anmeldelse] FOREIGN KEY ([ModtagerAnlaegId]) REFERENCES [dbo].[ModtagerAnlaeg] ([Id]),
    CONSTRAINT [Kommune_Anmeldelse] FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id]),
    CONSTRAINT [Sagsbehandler_Anmeldelse] FOREIGN KEY ([SagsbehandlerId]) REFERENCES [dbo].[Sagsbehandler] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_1]
    ON [dbo].[Anmeldelse]([Nummer] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_1_Recommended]
    ON [dbo].[Anmeldelse]([KommuneId] ASC, [Id] ASC)
    INCLUDE([TransportoerId], [AnmelderId], [ModtagerAnlaegId], [BetalerId], [SagsbehandlerId], [RevisionAfAnmeldelse], [Nummer], [BemaerkningTilAnmeldelse], [BemaerkningTilKommune], [BemaerkningTilJordmodtager], [AarsagAfvisning], [BemarkningInternKommune], [KoertJordAlarm], [AnmelderSagsnummer], [FlagSagsbehandler]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_2_FK]
    ON [dbo].[Anmeldelse]([KommuneId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_2_Recommended]
    ON [dbo].[Anmeldelse]([AnmelderId] ASC, [ModtagerAnlaegId] ASC, [Id] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_3_FK]
    ON [dbo].[Anmeldelse]([TransportoerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_3_Recommended]
    ON [dbo].[Anmeldelse]([AnmelderId] ASC, [Id] ASC, [TransportoerId] ASC)
    INCLUDE([ModtagerAnlaegId]);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_4_FK]
    ON [dbo].[Anmeldelse]([AnmelderId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_4_Recommended]
    ON [dbo].[Anmeldelse]([SagsbehandlerId] ASC, [Id] ASC)
    INCLUDE([KommuneId], [TransportoerId], [AnmelderId], [ModtagerAnlaegId], [BetalerId], [RevisionAfAnmeldelse], [Nummer], [BemaerkningTilAnmeldelse], [BemaerkningTilKommune], [BemaerkningTilJordmodtager], [AarsagAfvisning], [BemarkningInternKommune], [KoertJordAlarm], [AnmelderSagsnummer], [FlagSagsbehandler]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_5_FK]
    ON [dbo].[Anmeldelse]([ModtagerAnlaegId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_5_Recommended]
    ON [dbo].[Anmeldelse]([ModtagerAnlaegId] ASC, [Id] ASC)
    INCLUDE([KommuneId], [TransportoerId], [AnmelderId], [BetalerId], [SagsbehandlerId], [RevisionAfAnmeldelse], [Nummer], [BemaerkningTilAnmeldelse], [BemaerkningTilKommune], [BemaerkningTilJordmodtager], [AarsagAfvisning], [BemarkningInternKommune], [KoertJordAlarm], [AnmelderSagsnummer], [FlagSagsbehandler]) WITH (FILLFACTOR = 80);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_6_FK]
    ON [dbo].[Anmeldelse]([BetalerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_7_FK]
    ON [dbo].[Anmeldelse]([SagsbehandlerId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Anmeldelse_8_FK]
    ON [dbo].[Anmeldelse]([RevisionAfAnmeldelse] ASC);


GO

