CREATE TABLE [dbo].[Jord] (
    [Id]                                UNIQUEIDENTIFIER NOT NULL,
    [JordKlassifikationTypeId]          UNIQUEIDENTIFIER NULL,
    [AffaldTypeId]                      UNIQUEIDENTIFIER NULL,
    [JordflytningTypeId]                UNIQUEIDENTIFIER NULL,
    [MiljoeTekniskTilsyn]               NVARCHAR (100)   NULL,
    [Jordproever]                       BIT              NULL,
    [JordproeverFoer]                   BIT              NULL,
    [ForventetJordmaengdeTon]           NUMERIC (10)     NULL,
    [KoerselStart]                      DATE             NULL,
    [KoerselSlut]                       DATE             NULL,
    [AndenAffaldType]                   NVARCHAR (100)   NULL,
    [Bemaerkning]                       NVARCHAR (MAX)   NULL,
    [JordarbejdeBeskrivelse]            NVARCHAR (MAX)   NULL,
    [TidligereErhvervsaktivitet]        NVARCHAR (MAX)   NULL,
    [IntaktJord]                        BIT              NOT NULL,
    [AkutBaggrund]                      NVARCHAR (100)   NULL,
    [StraksGodkendJordhaandteringsplan] BIT              NULL,
    [AntalProever]                      NUMERIC (3)      NULL,
    [LinkTilGodkendtAnmeldelse]         NVARCHAR (1024)  NULL,
    [EgetOrdrenummer]                   NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Jord] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [AffaldType_Jord] FOREIGN KEY ([AffaldTypeId]) REFERENCES [dbo].[AffaldType] ([Id]),
    CONSTRAINT [Anmeldelse_Jord] FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [JordarbejdeKlassifikationType_Jordarbejde] FOREIGN KEY ([JordKlassifikationTypeId]) REFERENCES [dbo].[JordKlassifikationType] ([Id]),
    CONSTRAINT [JordflytningType_Jord] FOREIGN KEY ([JordflytningTypeId]) REFERENCES [dbo].[JordflytningType] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IDX_Jord_1_FK]
    ON [dbo].[Jord]([JordKlassifikationTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Jord_2_FK]
    ON [dbo].[Jord]([AffaldTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Jord_3_FK]
    ON [dbo].[Jord]([JordflytningTypeId] ASC);


GO

