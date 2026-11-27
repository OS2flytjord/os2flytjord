CREATE TABLE [dbo].[Jordforureningsopslag] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [JordKlassifikationTypeId] UNIQUEIDENTIFIER NOT NULL,
    [V2]                       BIT              NOT NULL,
    [V1]                       BIT              NOT NULL,
    [OmkAnalysepligt]          BIT              NOT NULL,
    [OmkLet]                   BIT              NOT NULL,
    [OmkRen]                   BIT              NOT NULL,
    [KommunensMiljoeDb]        NVARCHAR (MAX)   NULL,
    [Tid]                      DATETIME         NOT NULL,
    CONSTRAINT [PK_Jordforureningsopslag] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Anmeldelse_Jordforureningsopslag] FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [JordKlassifikationType_Jordforureningsopslag] FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id])
);


GO
ALTER TABLE [dbo].[Jordforureningsopslag] NOCHECK CONSTRAINT [Anmeldelse_Jordforureningsopslag];


GO
ALTER TABLE [dbo].[Jordforureningsopslag] NOCHECK CONSTRAINT [JordKlassifikationType_Jordforureningsopslag];


GO

CREATE NONCLUSTERED INDEX [IDX_Jordforureningsopslag_1_FK]
    ON [dbo].[Jordforureningsopslag]([JordKlassifikationTypeId] ASC);


GO

