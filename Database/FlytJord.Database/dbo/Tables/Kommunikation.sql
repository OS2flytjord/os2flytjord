CREATE TABLE [dbo].[Kommunikation] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DEF_Kommunikation_Id] DEFAULT (newid()) NOT NULL,
    [FraPerson]    UNIQUEIDENTIFIER NOT NULL,
    [TilPerson]    UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId] UNIQUEIDENTIFIER NOT NULL,
    [BeskedId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Kommunikation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Anmeldelse_Kommunikation] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [Besked_Kommunikation] FOREIGN KEY ([BeskedId]) REFERENCES [dbo].[Besked] ([Id]),
    CONSTRAINT [Person_KommunikationFra] FOREIGN KEY ([FraPerson]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [Person_KommunikationTil] FOREIGN KEY ([TilPerson]) REFERENCES [dbo].[Person] ([Id])
);


GO
ALTER TABLE [dbo].[Kommunikation] NOCHECK CONSTRAINT [Anmeldelse_Kommunikation];


GO
ALTER TABLE [dbo].[Kommunikation] NOCHECK CONSTRAINT [Besked_Kommunikation];


GO
ALTER TABLE [dbo].[Kommunikation] NOCHECK CONSTRAINT [Person_KommunikationFra];


GO
ALTER TABLE [dbo].[Kommunikation] NOCHECK CONSTRAINT [Person_KommunikationTil];


GO

CREATE NONCLUSTERED INDEX [IDX_Kommunikation_1_FK]
    ON [dbo].[Kommunikation]([FraPerson] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Kommunikation_2_FK]
    ON [dbo].[Kommunikation]([TilPerson] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Kommunikation_3_FK]
    ON [dbo].[Kommunikation]([AnmeldelseId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Kommunikation_4_FK]
    ON [dbo].[Kommunikation]([BeskedId] ASC);


GO

