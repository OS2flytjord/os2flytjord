CREATE TABLE [dbo].[Matrikel] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DEF_Matrikel_Id] DEFAULT (newid()) NOT NULL,
    [OprindelsesstedId] UNIQUEIDENTIFIER NULL,
    [Matrikelnr]        VARCHAR (40)     NULL,
    [Ejerlav]           VARCHAR (40)     NULL,
    [Ejerlavsnavn]      VARCHAR (40)     NULL,
    [Sogn]              VARCHAR (40)     NULL,
    [Herred]            VARCHAR (40)     NULL,
    [Dato]              DATE             NULL,
    [Geom]              [sys].[geometry] NULL,
    CONSTRAINT [PK_Matrikel] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Oprindelsessted_Matrikel] FOREIGN KEY ([OprindelsesstedId]) REFERENCES [dbo].[Oprindelsessted] ([Id])
);


GO
ALTER TABLE [dbo].[Matrikel] NOCHECK CONSTRAINT [Oprindelsessted_Matrikel];


GO

CREATE NONCLUSTERED INDEX [IDX_Matrikel_2_FK]
    ON [dbo].[Matrikel]([OprindelsesstedId] ASC);


GO

CREATE SPATIAL INDEX [SPIDX_Matrikel]
    ON [dbo].[Matrikel] ([Geom])
    USING GEOMETRY_GRID
    WITH  (
            BOUNDING_BOX = (XMAX = 796336, XMIN = 424358, YMAX = 6418559, YMIN = 6033856),
            GRIDS = (LEVEL_1 = MEDIUM, LEVEL_2 = MEDIUM, LEVEL_3 = MEDIUM, LEVEL_4 = MEDIUM),
            FILLFACTOR = 80
          );


GO

