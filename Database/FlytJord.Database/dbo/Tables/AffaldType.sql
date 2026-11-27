CREATE TABLE [dbo].[AffaldType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF__AffaldType__Id__345EC57D] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Aktiv]     BIT              NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_AffaldType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AffaldType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

