CREATE TABLE [dbo].[Oprindelsessted] (
    [Id]                                  UNIQUEIDENTIFIER NOT NULL,
    [OprindelsesstedKlassifikationTypeId] UNIQUEIDENTIFIER NULL,
    [Adresse]                             NVARCHAR (200)   NULL,
    [Postnummer]                          NUMERIC (4)      NULL,
    [PostDistrikt]                        NVARCHAR (50)    NULL,
    [TidligereErhvervsAktivitet]          NVARCHAR (100)   NULL,
    [Kortlagt]                            NVARCHAR (40)    NULL,
    [Geom]                                [sys].[geometry] NULL,
    [OffvejUrl]                           NVARCHAR (500)   NULL,
    [Beskrivelse]                         NVARCHAR (500)   NULL,
    [AndenOprindJordTypeID]               UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Oprindelsessted] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Anmeldelse_Oprindelsessted] FOREIGN KEY ([Id]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [FK__Oprindels__Anden__72B0FDB1] FOREIGN KEY ([AndenOprindJordTypeID]) REFERENCES [dbo].[AndenOprindJordType] ([Id]),
    CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted] FOREIGN KEY ([OprindelsesstedKlassifikationTypeId]) REFERENCES [dbo].[OprindelsesstedKlassifikationType] ([Id])
);


GO
ALTER TABLE [dbo].[Oprindelsessted] NOCHECK CONSTRAINT [Anmeldelse_Oprindelsessted];


GO
ALTER TABLE [dbo].[Oprindelsessted] NOCHECK CONSTRAINT [FK__Oprindels__Anden__72B0FDB1];


GO
ALTER TABLE [dbo].[Oprindelsessted] NOCHECK CONSTRAINT [OprindelsesstedKlassifikationType_Oprindelsessted];


GO

CREATE NONCLUSTERED INDEX [IDX_Oprindelsessted_1_FK]
    ON [dbo].[Oprindelsessted]([OprindelsesstedKlassifikationTypeId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Oprindelsessted_2_FK]
    ON [dbo].[Oprindelsessted]([AndenOprindJordTypeID] ASC);


GO

CREATE SPATIAL INDEX [SPIDX_Oprindelsessted_Geom]
    ON [dbo].[Oprindelsessted] ([Geom])
    USING GEOMETRY_GRID
    WITH  (
            BOUNDING_BOX = (XMAX = 796336, XMIN = 424358, YMAX = 6418559, YMIN = 6033856),
            GRIDS = (LEVEL_1 = MEDIUM, LEVEL_2 = MEDIUM, LEVEL_3 = MEDIUM, LEVEL_4 = MEDIUM)
          );


GO

