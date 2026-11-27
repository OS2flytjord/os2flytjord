CREATE TABLE [dbo].[Advis] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Advis_Id] DEFAULT (newid()) NOT NULL,
    [AdvisTypeId]  UNIQUEIDENTIFIER NOT NULL,
    [PersonId]     UNIQUEIDENTIFIER NULL,
    [BeskedId]     UNIQUEIDENTIFIER NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Advis] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [AdvisType_Advis] FOREIGN KEY ([AdvisTypeId]) REFERENCES [dbo].[AdvisType] ([Id]),
    CONSTRAINT [Anmeldelse_Advis] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Besked_Advis] FOREIGN KEY ([BeskedId]) REFERENCES [dbo].[Besked] ([Id]),
    CONSTRAINT [Person_Advis] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Advis_1_FK]
    ON [dbo].[Advis]([AdvisTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Advis_2_FK]
    ON [dbo].[Advis]([PersonId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Advis_3_FK]
    ON [dbo].[Advis]([BeskedId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Advis_4_FK]
    ON [dbo].[Advis]([AnmeldelseId] ASC);


GO

