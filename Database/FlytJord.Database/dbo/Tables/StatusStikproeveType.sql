CREATE TABLE [dbo].[StatusStikproeveType] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF__StatusStikpr__Id__3B0BC30C] DEFAULT (newid()) NOT NULL,
    [Navn]      NVARCHAR (40)    NOT NULL,
    [Sortering] NUMERIC (2)      NULL,
    [Aktiv]     BIT              CONSTRAINT [DEF_StatusStikproeveType_Aktiv] DEFAULT ((1)) NOT NULL,
    [Kode]      SMALLINT         CONSTRAINT [DEF_StatusStikproeveType_Kode] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StatusStikproeveType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

