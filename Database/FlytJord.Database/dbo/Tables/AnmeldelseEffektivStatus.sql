CREATE TABLE [dbo].[AnmeldelseEffektivStatus] (
    [AnmeldelseId]             UNIQUEIDENTIFIER NOT NULL,
    [Oprettet]                 DATETIME         NULL,
    [Gemt]                     DATETIME         NULL,
    [Afsendt]                  DATETIME         NULL,
    [BetalingAccepteret]       DATETIME         NULL,
    [BetalingAfvist]           DATETIME         NULL,
    [UnderBehandling]          DATETIME         NULL,
    [Godkendt]                 DATETIME         NULL,
    [Afvist]                   DATETIME         NULL,
    [JordAccepteret]           DATETIME         NULL,
    [JordAfvist]               DATETIME         NULL,
    [Afsluttet]                DATETIME         NULL,
    [Revideret]                DATETIME         NULL,
    [Aktiv]                    DATETIME         NULL,
    [ForureningsstatusAendret] DATETIME         NULL,
    CONSTRAINT [PK_AnmeldelseEffektivStatus] PRIMARY KEY CLUSTERED ([AnmeldelseId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_AnmeldelseEffektivStatus_Anmeldelse] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]) ON DELETE CASCADE
);


GO

