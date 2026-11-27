CREATE TABLE [dbo].[Opgavetype] (
    [Id]                         UNIQUEIDENTIFIER CONSTRAINT [DF_Opgavetype_Id] DEFAULT (newid()) NOT NULL,
    [KommuneId]                  UNIQUEIDENTIFIER NOT NULL,
    [Kode]                       INT              NOT NULL,
    [Navn]                       NVARCHAR (200)   NOT NULL,
    [Timepris]                   NUMERIC (10, 2)  NOT NULL,
    [FraDato]                    DATETIME         NULL,
    [TilDato]                    DATETIME         NULL,
    [Afrunding]                  INT              NOT NULL,
    [Sortering]                  INT              NOT NULL,
    [KMDOpusMaterialeVareNummer] INT              NULL,
    [KMDOpusPSPElementNummer]    NVARCHAR (24)    NULL,
    CONSTRAINT [PK_Opgavetype] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Opgavetype_Kommune] FOREIGN KEY ([KommuneId]) REFERENCES [dbo].[Kommune] ([Id])
);


GO
ALTER TABLE [dbo].[Opgavetype] NOCHECK CONSTRAINT [FK_Opgavetype_Kommune];


GO

