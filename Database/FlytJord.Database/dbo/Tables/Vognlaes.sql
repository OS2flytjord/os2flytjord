CREATE TABLE [dbo].[Vognlaes] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF_Vognlaes_Id] DEFAULT (newid()) NOT NULL,
    [LastbilId]     UNIQUEIDENTIFIER NOT NULL,
    [AnmeldelseId]  UNIQUEIDENTIFIER NULL,
    [Dato]          DATETIME         NOT NULL,
    [MaengdeTon]    NUMERIC (10, 3)  NULL,
    [MaengdeAksler] NUMERIC (10)     NULL,
    [Afvist]        BIT              CONSTRAINT [DEF_Vognlaes_Afvist] DEFAULT ((0)) NOT NULL,
    [AfvistNote]    NVARCHAR (256)   NULL,
    CONSTRAINT [PK_Vognlaes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Anmeldelse_Vognlaes] FOREIGN KEY ([AnmeldelseId]) REFERENCES [dbo].[Anmeldelse] ([Id]),
    CONSTRAINT [FK_Vognlaes_Lastbil] FOREIGN KEY ([LastbilId]) REFERENCES [dbo].[Lastbil] ([Id])
);


GO
ALTER TABLE [dbo].[Vognlaes] NOCHECK CONSTRAINT [Anmeldelse_Vognlaes];


GO
ALTER TABLE [dbo].[Vognlaes] NOCHECK CONSTRAINT [FK_Vognlaes_Lastbil];


GO

CREATE NONCLUSTERED INDEX [IDX_Vognlaes_1]
    ON [dbo].[Vognlaes]([Dato] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Vognlaes_1_Recommended]
    ON [dbo].[Vognlaes]([AnmeldelseId] ASC, [Id] ASC)
    INCLUDE([LastbilId], [Dato], [MaengdeTon], [MaengdeAksler], [Afvist], [AfvistNote]);


GO

CREATE NONCLUSTERED INDEX [IDX_Vognlaes_2_FK]
    ON [dbo].[Vognlaes]([LastbilId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Vognlaes_3_FK]
    ON [dbo].[Vognlaes]([AnmeldelseId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_Vognlaes_4]
    ON [dbo].[Vognlaes]([Afvist] ASC);


GO

