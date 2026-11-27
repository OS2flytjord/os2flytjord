CREATE TABLE [dbo].[Dokumentation] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_Dokumentation_Id] DEFAULT (newid()) NOT NULL,
    [DokumentationTypeId] UNIQUEIDENTIFIER NULL,
    [Filnavn]             NVARCHAR (100)   NOT NULL,
    [JordId]              UNIQUEIDENTIFIER NULL,
    [OprindelsesDato]     DATETIME         NOT NULL,
    CONSTRAINT [PK_Dokumentation] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [DokumentationType_Dokumentation] FOREIGN KEY ([DokumentationTypeId]) REFERENCES [dbo].[DokumentationType] ([Id]),
    CONSTRAINT [Jord_Dokumentation] FOREIGN KEY ([JordId]) REFERENCES [dbo].[Jord] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Dokumentation_1_FK]
    ON [dbo].[Dokumentation]([DokumentationTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Dokumentation_2_FK]
    ON [dbo].[Dokumentation]([JordId] ASC);


GO

