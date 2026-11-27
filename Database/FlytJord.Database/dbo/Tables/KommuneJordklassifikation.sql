CREATE TABLE [dbo].[KommuneJordklassifikation] (
    [Id]                       UNIQUEIDENTIFIER CONSTRAINT [DEF_KommuneJordklassifikation_Id] DEFAULT (newid()) NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [KommuneId]                UNIQUEIDENTIFIER NOT NULL,
    [Aktiv]                    BIT              NOT NULL,
    CONSTRAINT [PK_KommuneJordklassifikation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation] FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id]),
    CONSTRAINT [Kommune_KommuneJordklassifikation] FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
);


GO
ALTER TABLE [dbo].[KommuneJordklassifikation] NOCHECK CONSTRAINT [JordKlassifikationType_KommuneJordklassifikation];


GO
ALTER TABLE [dbo].[KommuneJordklassifikation] NOCHECK CONSTRAINT [Kommune_KommuneJordklassifikation];


GO

CREATE NONCLUSTERED INDEX [IDX_KommuneJordklassifikation_1_FK]
    ON [dbo].[KommuneJordklassifikation]([JordKlassifikationTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_KommuneJordklassifikation_2_FK]
    ON [dbo].[KommuneJordklassifikation]([KommuneId] ASC);


GO

